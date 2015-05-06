using System.Web.Security;
using System.Collections.Generic;

namespace Oogstplanner
{
    public static class MembershipHelper
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.

            var errorcodes = new Dictionary<MembershipCreateStatus, string>()
            {
                { MembershipCreateStatus.DuplicateUserName, 
                    "Gebruikersnaam bestaat al. Vult u alstublieft een andere gebruikersnaam in." },
                { MembershipCreateStatus.DuplicateEmail, 
                    "Er bestaat al een gebruiker met dit e-mailadres. Vult u alstublieft een ander e-mailadres in." },
                { MembershipCreateStatus.InvalidPassword, 
                    "Het wachtwoord is niet geldig. Vult u alstublieft een geldig wachtwoord in." },
                { MembershipCreateStatus.InvalidEmail, 
                    "Het verstrekte e-mailadres is ongeldig. Controleert u alstublieft het adres en vul een correcte in." },
                { MembershipCreateStatus.InvalidAnswer, 
                    "Het antwoord op de geheime vraag is ongeldig. Controleert u alstublieft de waarde en probeer het opnieuw." },
                { MembershipCreateStatus.InvalidQuestion, 
                    "De geheime vraag is ongeldig. Controleert u alstublieft de waarde en probeer het opnieuw." },
                { MembershipCreateStatus.InvalidUserName, 
                    "De verstrekte gebruikersnaam is ongeldig. Controleert u alstublieft de waarde en probeer het opnieuw." },
                { MembershipCreateStatus.ProviderError, 
                    "De verstrekte authenticatie leidde tot een fout. Controleert u alstublieft de waarde en probeer het opnieuw." +
                    " Als het probleem aanhoudt contacteer dan oogstplanner@gmail.com." },
                { MembershipCreateStatus.UserRejected, 
                    "Het creëeren van een nieuwe gebruiker is mislukt. Controleert u alstublieft de waarde en probeer het " +
                    "opnieuw. Als het probleem aanhoudt contacteer dan oogstplanner@gmail.com." }
            };

            return errorcodes.ContainsKey(createStatus) 
                ? errorcodes[createStatus] 
                : "Een onbekende fout heeft plaatsgevonden. Controleert u alstublieft de waarde en probeer het opnieuw. " +
                  "Als het probleem aanhoudt contacteer dan oogstplanner@gmail.com.";
        }
    }
}    