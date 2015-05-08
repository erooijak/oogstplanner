using System;
using System.Linq;
using System.Web.Security;
using System.Collections.Generic;

using Oogstplanner.Models;

namespace Oogstplanner.Utilities.Helpers
{
    public static class MembershipHelper
    {
        // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        // a full list of status codes.

        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            var errorCodes = new Dictionary<MembershipCreateStatus, string>()
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

            return errorCodes.ContainsKey(createStatus) 
                ? errorCodes[createStatus] 
                : "Een onbekende fout heeft plaatsgevonden. Controleert u alstublieft de waarde en probeer het opnieuw. " +
                  "Als het probleem aanhoudt contacteer dan oogstplanner@gmail.com.";
        }

        /// <summary>
        /// Creates the key to which the error message is attached.
        /// </summary>
        /// <remarks>
        /// Used on the view to highlight the failing fields.
        /// IMPORTANT: If registermodel property names change this should be changed too.
        /// </remarks>
        /// <returns>The the name of the property which caused the error or empty string if not found</returns>
        /// <param name="createStatus">Create status.</param>
        public static string ErrorCodeToKey(MembershipCreateStatus createStatus)
        {
            var errorCodeKeys = new Dictionary<MembershipCreateStatus, string>()
            {
                { MembershipCreateStatus.DuplicateUserName, 
                    "UserName" },
                { MembershipCreateStatus.InvalidUserName, 
                    "UserName" },
                { MembershipCreateStatus.DuplicateEmail, 
                    "Email" },                
                { MembershipCreateStatus.InvalidEmail, 
                    "Email" },
                { MembershipCreateStatus.InvalidPassword, 
                    "Password" },
            };

            return errorCodeKeys.ContainsKey(createStatus) ? errorCodeKeys[createStatus] : "";
        }
    }
}    