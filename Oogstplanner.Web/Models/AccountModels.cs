using System;
using System.ComponentModel.DataAnnotations;

namespace Oogstplanner.Models
{
    public class RegisterExternalLoginModel
    {
        [Display(Name = "Naam")]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Huidig wachtwoord")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het wachtwoord moet op zijn minst {2} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig nieuwe wachtwoord")]
        [Compare("NewPassword", ErrorMessage = "De twee wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Gebruikersnaam")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Onthoud mij?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Naam is verplicht.")]
        [Display(Name = "Naam")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Gebruikersnaam is verplicht.")]
        [Display(Name = "Gebruikersnaam")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [Display(Name = "E-mailadres")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [StringLength(100, ErrorMessage = "Het wachtwoord moet op zijn minst {2} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

    }

    public class LostPasswordModel
    {
        [Required(ErrorMessage = "We hebben uw e-mail nodig om u een resetlink te kunnen sturen!")]
        [Display(Name = "Het emailadres dat bij uw account hoort")]
        [EmailAddress(ErrorMessage= "Geen valide e-mailadres.")]
        public string Email { get; set; }
    }

    /* For storage of tokens in database */
    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    /* For display on the view */
    public class ResetPasswordModel
    {
        [Required]
        [Display(Name = "Nieuw wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Bevestig wachtwoord")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wachtwoorden zijn niet hetzelfde.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ReturnToken { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
        
}