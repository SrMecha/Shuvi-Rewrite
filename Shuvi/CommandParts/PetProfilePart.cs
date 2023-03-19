using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class PetProfilePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("profilePart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IDatabasePet pet, bool canEdit = false)
        {
            while (context.LastInteraction is not null)
            {

            }
        }
    }
}
