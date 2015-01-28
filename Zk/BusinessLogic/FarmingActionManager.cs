using System;
using System.Collections.Generic;
using System.Linq;

using Zk.Models;
using Zk.Repositories;

namespace Zk.BusinessLogic
{
    public class FarmingActionManager
    {
        readonly Repository _repository;

        public FarmingActionManager()
        {
            _repository = new Repository();
        }

        public FarmingActionManager(Repository repository)
        {
            _repository = repository;
        }

        public IEnumerable<FarmingAction> GetHarvestingActions(Month month)
        {
            return _repository.GetFarmingActions(
                fa => fa.Action == FarmType.Harvesting && fa.Month.HasFlag(month));
        }

        public IEnumerable<FarmingAction> GetSowingActions(Month month)
        {
            return _repository.GetFarmingActions(
                fa => fa.Action == FarmType.Sowing && fa.Month.HasFlag(month));
        }

        public void UpdateCropCounts(IList<int> ids, IList<int> counts)
        {
            if (ids.Count != counts.Count) throw new ArgumentException(
                "Different amount of ids and counts.", "counts");

            // Combine each farming id to it's respective farming count in a keyvaluepair (kvp)
            // where the id is the key and cropCount the value.
            foreach (var kvp in ids.Zip(counts, (id, count) => new KeyValuePair<int, int>(id, count)))
            {
                var action = _repository.FindFarmingAction(kvp.Key);

                var oldCropCount = action.CropCount;
                var newCropCount = kvp.Value;

                if (oldCropCount == newCropCount) continue;

                // TODO:    Implement logic to update all related farming actions.
                // AKA:     The calculation.

                // Opletten of hij al bestaat (een actie met die crop, die user, die maand, met alleen ander aantal)
                // en enkel dient te worden geupdatet. 
                // Oude updaten

                // Nieuwe farming action aanmaken. Met tegenovergestelde action 
                // (e.g., indien sowing dan harvesting)
                // if sowing dan +, if harvesting dan -
                var newFarmingAction = new FarmingAction();

                newFarmingAction.Action = action.Action == FarmType.Harvesting 
                    ? newFarmingAction.Action = FarmType.Sowing
                    : newFarmingAction.Action = FarmType.Harvesting;

                newFarmingAction.Month = action.Action == FarmType.Sowing
                    ? action.Month + action.Crop.GrowingTime
                    : action.Month - action.Crop.GrowingTime;
 
                newFarmingAction.Crop = action.Crop;
                newFarmingAction.CropCount = newCropCount;
                newFarmingAction.Calendar = action.Calendar;

                // Het aantal is opgegeven aantal. De crop is de aangegeven crop.


                // Update one crop count of a farming action in the database.
                action.CropCount = kvp.Value;
                _repository.Update(action);
            }

            _repository.SaveChanges();
        }

    }
}