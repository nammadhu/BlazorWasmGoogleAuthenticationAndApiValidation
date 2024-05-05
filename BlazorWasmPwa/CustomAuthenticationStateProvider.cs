using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Components.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

//using BlazorEssentials.Storage;
using Blazored.LocalStorage;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
	private readonly ILocalStorageService _localStorage;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
		{
		_localStorage = localStorage;
		}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
		var savedToken = await _localStorage.GetItemAsync<string>("authToken");

		if (string.IsNullOrWhiteSpace(savedToken))
			{
			return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
			}

		var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
			ParseClaimsFromUri(savedToken), "Jwt"));

		// Optional: Validate token expiration or other security checks here
		// before returning claimsPrincipal

		return await Task.FromResult(new AuthenticationState(claimsPrincipal));
		}

	public async Task MarkUserAsAuthenticated(string token)
		{
		await _localStorage.SetItemAsync("authToken", token);
		NotifyAuthenticationStateChanged( GetAuthenticationStateAsync());
		}

	public async Task MarkUserAsLoggedOut()
		{
		await _localStorage.RemoveItemAsync("authToken");
		NotifyAuthenticationStateChanged( GetAuthenticationStateAsync());
		}

	private static IEnumerable<Claim> ParseClaimsFromUri(string uri)
		{
		var elements = uri.Split('.');

		try
			{
			var payload = Base64UrlEncoder.Decode(elements[1]);
			var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);
			return claims.Select(claim => new Claim(claim.Key, claim.Value));
			}
		catch (Exception)
			{
			return new List<Claim>();
			}
		}
	}



