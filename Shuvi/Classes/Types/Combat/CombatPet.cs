﻿using Shuvi.Classes.Types.Characteristics;
using Shuvi.Classes.Types.Characteristics.Dynamic;
using Shuvi.Classes.Types.Pet;
using Shuvi.Interfaces.Characteristics.Dynamic;
using Shuvi.Interfaces.Combat;

namespace Shuvi.Classes.Types.Combat
{
    public class CombatPet : CombatEntity, ICombatPet
    {
        public CombatPet(DatabasePet pet)
        {
            Name = pet.Name;
            Rank = pet.Rank;
            Characteristics = new EntityCharacteristics<INotRestorableCharacteristic>()
            {
                Strength = pet.Characteristics.Strength,
                Agility = pet.Characteristics.Agility,
                Luck = pet.Characteristics.Luck,
                Intellect = pet.Characteristics.Intellect,
                Endurance = pet.Characteristics.Endurance,
                Health = new NotRestorableCharacteristic(pet.Characteristics.Health.GetCurrent(), pet.Characteristics.Health.GetCurrent()),
                Mana = new NotRestorableCharacteristic(pet.Characteristics.Mana.GetCurrent(), pet.Characteristics.Mana.GetCurrent())
            };
            Spell = pet.Spell.GetSpell();
            Actions = pet.ActionChances;
        }
    }
}
