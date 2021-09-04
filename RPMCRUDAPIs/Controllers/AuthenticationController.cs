using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPMCRUDAPIs.Models;

namespace RPMCRUDAPIs.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private const string _clientId = "2gq0d7i7a6rul0q0ai7l7dsids";
        private readonly RegionEndpoint _region = RegionEndpoint.USWest2;

        [HttpPost]
        [Route("api/register")]
        public async Task<ActionResult<string>> Register(User user)
        {
            try
            {
                var cognito = new AmazonCognitoIdentityProviderClient(_region);

                var request = new SignUpRequest
                {
                    ClientId = _clientId,
                    Password = user.Password,
                    Username = user.Username
                };

                var emailAttribute = new AttributeType
                {
                    Name = "email",
                    Value = user.Email
                };
                request.UserAttributes.Add(emailAttribute);

                var response = await cognito.SignUpAsync(request);

                return Ok("Registered");
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/signin")]
        public async Task<ActionResult<AuthModel>> SignIn(User user)
        {
            try
            {
                var cognito = new AmazonCognitoIdentityProviderClient(_region);

                var request = new AdminInitiateAuthRequest
                {
                    UserPoolId = "us-west-2_5A1B8tLc9",
                    ClientId = _clientId,
                    AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
                };

                request.AuthParameters.Add("USERNAME", user.Username);
                request.AuthParameters.Add("PASSWORD", user.Password);

                var response = await cognito.AdminInitiateAuthAsync(request);
                if (response.HttpStatusCode.ToString() == "OK")
                {
                    AuthModel authValues = new AuthModel();
                    authValues.IdToken = response.AuthenticationResult.IdToken;
                    authValues.Expiresin = response.AuthenticationResult.ExpiresIn;
                    authValues.RefreshToken = response.AuthenticationResult.RefreshToken;
                    authValues.TokenType = response.AuthenticationResult.TokenType;
                    return Ok(authValues);
                }
                else
                    return Ok("Unauthorized");
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/confirmsignup")]
        public async Task<ActionResult<string>> ConfirmSignUp(ConfirmUser user)
        {
            try
            {
                var cognito = new AmazonCognitoIdentityProviderClient(_region);

                var request = new ConfirmSignUpRequest
                {
                    ClientId = _clientId,
                    ConfirmationCode = user.code,
                    Username = user.Username
                };

                var response = await cognito.ConfirmSignUpAsync(request);

                return Ok("User Confirmed");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/resendcode")]
        public async Task<ActionResult<string>> ResendConfirmationCode(ResendCode reuser)
        {
            try
            {
                var cognito = new AmazonCognitoIdentityProviderClient(_region);

                var request = new ResendConfirmationCodeRequest
                {
                    ClientId = _clientId,
                    Username = reuser.Username
                };

                var response = await cognito.ResendConfirmationCodeAsync(request);

                return Ok("Code sent to "+ response.CodeDeliveryDetails.Destination);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/forgotpassword")]
        public async Task<ActionResult<string>> ForgotPassword(User user)
        {
            try
            {
                var cognito = new AmazonCognitoIdentityProviderClient(_region);

                var request = new ForgotPasswordRequest
                {
                    ClientId = _clientId,
                    Username = user.Username
                };

                var response = await cognito.ForgotPasswordAsync(request);

                return Ok("Password reset code sent to " + response.CodeDeliveryDetails.Destination);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/confirmforgotpassword")]
        public async Task<ActionResult<string>> ConfirmForgotPassword(ConfirmForgotPassword confirmForgotPassword)
        {
            try
            {
                var cognito = new AmazonCognitoIdentityProviderClient(_region);
                var request = new ConfirmForgotPasswordRequest
                {
                    ClientId = _clientId,
                    Username = confirmForgotPassword.username,
                    Password = confirmForgotPassword.newpassword,
                    ConfirmationCode = confirmForgotPassword.confirmationcode
                };

                var response = await cognito.ConfirmForgotPasswordAsync(request);

                if(response.HttpStatusCode.ToString()=="OK")
                return Ok("Password reset successfully..!");
                else { return Ok(response); }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}