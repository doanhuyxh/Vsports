﻿using System.ComponentModel.DataAnnotations;

namespace vsports.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string UserName { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("PasswordHash", ErrorMessage = "Mật khẩu không khớp")]
        public string? ConfirmPassword { get; set; }

        public static implicit operator ApplicationUser(RegisterViewModel vm)
        {
            return new ApplicationUser
            {
                UserName = vm.UserName,
                IsActive = true,
            };
        }
    }
}
