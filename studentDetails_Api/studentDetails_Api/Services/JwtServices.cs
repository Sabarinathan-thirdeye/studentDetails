﻿//using Microsoft.IdentityModel.Tokens;
//using studentDetails_Api.Models;
//using studentDetails_Api.NonEntity;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace studentDetails_Api.Services
//{
//    /// <summary>
//    /// Token Validation and Generation
//    /// </summary>
//    public class JwtServices
//    {
//        private const string ID = "studentID";
//        private readonly IConfiguration _config;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly string _signingKey = string.Empty;
//        private readonly string _encyptionKey = string.Empty;
//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="config"></param>
//        /// <param name="httpContextAccessor"></param>
//        public JwtServices(IConfiguration config, IHttpContextAccessor httpContextAccessor)
//        {
//            _config = config;
//            _httpContextAccessor = httpContextAccessor;
//            _signingKey = _config["JWT:SigningKey"] ?? "SignSecRetK3y$End!nE@PikEy!nS3ckEy";
//            _encyptionKey = _config["JWT:EncryptionKey"] ?? "EncSecRetK3y$vEnd!nE@PikEy!nS3ckEy";
//        }
//        /// <summary>
//        /// Get claims from the Auth Token
//        /// </summary>
//        /// <returns></returns>
//        public ClaimData GetClaimData()
//        {
//            ClaimData claimData = new ClaimData();
//            try
//            {
//                var symmetricSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_signingKey));
//                var symmetricEncKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_encyptionKey));
//                SecurityToken securityToken;
//                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

//                TokenValidationParameters validationParameters = new TokenValidationParameters()
//                {
//                    ValidAudience = _config["JWT:Audience"],
//                    ValidIssuer = _config["JWT:Issuer"],
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = symmetricSigningKey,
//                    TokenDecryptionKey = symmetricEncKey
//                };

//                // Get the HttpContext from IHttpContextAccessor
//                var httpContext = _httpContextAccessor.HttpContext;

//                // Extract the token from the Authorization header
//                var authorizationHeader = httpContext?.Request.Headers["Authorization"].FirstOrDefault();
//                var tokenString = authorizationHeader?.Replace("Bearer", "").Trim();

//                // Validate the token and retrieve the claims
//                ClaimsPrincipal objClaims = handler.ValidateToken(tokenString, validationParameters, out securityToken);

//                claimData = new ClaimData
//                {
//                    studentID = Convert.ToInt64(objClaims.Claims.First(claim => claim.Type == ID).Value),
//                    email = objClaims.Claims.First(claim => claim.Type == "email").Value,
//                    studentPassword = objClaims.Claims.First(claim => claim.Type == "studentPassword")?.Value ?? "NA",
//                    //UserPrivileges = JsonSerializer.Deserialize<List<PrivilegeMaster>>(objClaims.Claims.First(predicate: claim => claim.Type == "UserPrivileges").Value) ?? new List<PrivilegeMaster>()
//                };

//                // Set the ClaimData in HttpContext
//                httpContext!.Items["ClaimData"] = claimData;
//            }
//            catch (SecurityTokenExpiredException)
//            {
//                //throw new Exception("Security Token expired");
//            }
//            return claimData;
//        }

//        /// <summary>
//        /// Generates the JWT for the logged in user
//        /// </summary>
//        /// <param name="user"></param>
//        /// <returns></returns>
//        public string GenerateToken(User user)
//        {
//            var symmetricSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_signingKey));
//            var symmetricEncKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_encyptionKey));

//            var objClaims = new List<Claim>
//        {
//            //new Claim(ID, user.studentID.ToString()),
//            new Claim("Email", user.Email),
//            new Claim("studentPassword", user.Password),
//            // new Claim("UserPrivileges",JsonSerializer.Serialize(user.UserPrivileges)),

//            new Claim(JwtRegisteredClaimNames.Iss, _config!["JWT:Issuer"] ?? "studentDetails_Api"),
//            new Claim(JwtRegisteredClaimNames.Aud, _config !["JWT:Audience"] ?? "studentDetails_Api"),
//        };

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(objClaims),
//                Expires = DateTime.UtcNow.AddHours(24),
//                IssuedAt = DateTime.UtcNow,
//                Audience = _config["JWT:Audience"],
//                Issuer = _config["JWT:Issuer"],
//                SigningCredentials = new SigningCredentials(symmetricSigningKey, SecurityAlgorithms.HmacSha256Signature),
//                EncryptingCredentials = new EncryptingCredentials(symmetricEncKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes128CbcHmacSha256)
//            };
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
//            var tokenString = tokenHandler.WriteToken(token);

//            return tokenString;
//        }

//    }

//}