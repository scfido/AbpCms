// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace Cms.Passport.Web
{
    public class LoginInputModel
    {
        [Display(Name ="用户名")]
        [Required]
        public string Username { get; set; }

        [Required]
        [Display(Name ="密码")]
        public string Password { get; set; }

        [Display(Name ="记住登陆")]
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}