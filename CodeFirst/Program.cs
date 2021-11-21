using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace CodeFirst
{

    public class SavingAccount
    {
        [Key]
        public int AccountID { get; set; }
        public int TauxEpargne { get; set; }

        public int fond_initial { get; set; }

        public virtual Person Person { get; set; }
    }
    public class Person
    {
        [Key]
        public int PersonID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SavingAccount> SavingAccounts { get; set; }

    }

    public class SavingCalculator
    {

        public SavingAccount fond_initial { get; set; }
        public SavingAccount TauxEpargne{ get; set; }

        public static double CalculWithTauxPerMonths(int Taux, int fond_initial)
        {
            double TotauxEpargne = ((Taux * fond_initial) / 100) * 36;
            return TotauxEpargne;
        }

        public static double CalculWithTauxPerYears(int Taux, int fond_initial)
        {
            double TotauxEpargne = ((Taux * fond_initial) / 100) * 3;
            return TotauxEpargne;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MyContext1())
            {
                var personne = new Person
                {
                    Name = "Richard"
                };

                context.Persons.Add(personne);


                var Comptes = new List<SavingAccount>
                {
                new SavingAccount
                {
                    TauxEpargne = 5,
                    fond_initial = 2000000
                },
                new SavingAccount
                {
                    TauxEpargne = 15,
                    fond_initial = 250000
                },
                new SavingAccount
                {
                    TauxEpargne = 2,
                    fond_initial = 10000000
                },
                };

                foreach(var i in Comptes)
                {
                    context.SavingAccounts.Add(i);
                }


                for (int i = 1; i <= 3; i++)
                {
                    foreach (SavingAccount compteRichard in context.SavingAccounts.Distinct())//Garantie qu'il n'y a pas de doublons
                    {
                        if(compteRichard.TauxEpargne==5)
                        compteRichard.fond_initial = (int)SavingCalculator.CalculWithTauxPerMonths(compteRichard.TauxEpargne, compteRichard.fond_initial);
                        else
                        compteRichard.fond_initial = (int)SavingCalculator.CalculWithTauxPerYears(compteRichard.TauxEpargne, compteRichard.fond_initial);
                    }

                }

                context.SaveChanges();

                string message = "";

                foreach (SavingAccount compteRichard in context.SavingAccounts)
                {
                    if (compteRichard.TauxEpargne == 5)
                        message += $"\n  ID : {compteRichard.AccountID}, Solde : {compteRichard.fond_initial}, Taux d'épargne : {compteRichard.TauxEpargne}% chaque mois";
                    else
                        message += $"\n  ID : {compteRichard.AccountID}, Solde : {compteRichard.fond_initial}, Taux d'épargne : {compteRichard.TauxEpargne}% chaque année";
                }

                MessageBox.Show(message, "Solde des comptes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}
