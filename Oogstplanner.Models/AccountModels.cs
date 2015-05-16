using System;
using System.ComponentModel.DataAnnotations;

namespace Oogstplanner.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Gebruikersnaam of e-mailadres niet ingevuld.")]
        [Display(Name = "Gebruikersnaam of e-mailadres")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Wachtwoord niet ingevuld.")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Onthoud mij?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Naam is niet ingevuld.")]
        [Display(Name = "Naam")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Gebruikersnaam is niet ingevuld.")]
        [Display(Name = "Gebruikersnaam")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mailadres is niet ingevuld.")]
        [Display(Name = "E-mailadres")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is niet ingevuld.")]
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
        [Required(ErrorMessage = "Nieuw wachtwoord niet ingevuld.")]
        [Display(Name = "Nieuw wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bevestig wachtwoord niet ingevuld.")]
        [Display(Name = "Bevestig wachtwoord")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ReturnToken { get; set; }
    }        
}
