using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_I
{
    public class TransactionBancaire
    {
        public int RetraitMax = 1000;
        public int NombreRetraitMax = 9;
        public List<int[]> transaction;
        public Dictionary<int, decimal> compte;
        public List<int> transactionFait;

        public TransactionBancaire(string acctPath, string trxnPath)
        {

            // Lecture du fichier d'entré : transaction.
            string[] transactionLines = File.ReadAllLines(trxnPath);

            // Ecriture du fichier dans une liste de tableau.

            transaction = new List<int[]>();


            for (int i = 0; i < transactionLines.Length; i++)
            {
                int[] transactionInfo = new int[4];
                string[] split = transactionLines[i].Split(';');
                int numeroTrans = int.Parse(split[0]);
                int montant = int.Parse(split[1].Replace('.', ','));
                int compteExpediteur = int.Parse(split[2]);
                int compteDestinataire = int.Parse(split[3]);

                //Ajout de chaque champs dans le tableau de la liste
                transactionInfo[0] = numeroTrans;
                transactionInfo[1] = montant;
                transactionInfo[2] = compteExpediteur;
                transactionInfo[3] = compteDestinataire;
                transaction.Add(transactionInfo);
            }

            // Lecture du fichier d'entré : compte.
            string[] compteLines = File.ReadAllLines(acctPath);

            // Ecriture du fichier dans un dictionnaire.

            compte = new Dictionary<int, decimal>();
            for (int i = 0; i < compteLines.Length; i++)
            {
                string[] split = compteLines[i].Split(';');
                int numeroCompte = int.Parse(split[0]);
                decimal solde = !string.IsNullOrEmpty(split[1]) ? decimal.Parse(split[1].Replace('.', ',')) : 0;
                if (!compte.ContainsKey(numeroCompte))
                {
                    compte.Add(numeroCompte, solde);
                }
            }

        }


        // Vérification si un dépot est possible.
        public bool DepotEstPossible(int numeroTrans)
        {
            for (int i = 0; i < transaction.Count; i++)
            {
                if (transaction[i][0] == numeroTrans)
                {
                    int numeroCompte = transaction[i][3];
                    decimal soldeCompte = decimal.MinValue;

                    if (compte.ContainsKey(numeroCompte))
                    {
                        soldeCompte = compte[numeroCompte];
                    }
                    if (transaction[i][1] > 0 && soldeCompte >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        // Vérification si un retrait est possible.

        public bool RetraitEstPossible(int numeroTrans)
        {
            for (int i = 0; i < transaction.Count; i++)
            {
                if (transaction[i][0] == numeroTrans)
                {

                    int numeroCompte = transaction[i][2];
                    decimal soldeCompte;

                    if (compte.ContainsKey(numeroCompte))
                    {
                        soldeCompte = compte[numeroCompte];
                    }
                    else
                    {
                        return false;
                    }
                    if (transaction[i][1] > 0 && transaction[i][1] <= RetraitMax && transaction[i][1] <= soldeCompte)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Vérification du retrait maximum pour un virement.
        public bool RetraitMaxCumule(int numeroTrans)
        {
            int compteurRetrait = 0;
            decimal cumulRetrait = 0;
            int numeroCompte = 0;
            int indexTrans = 0;


            // Trouver la transaction et enregistrer son numero de compte
            for (int i = 0; i < transaction.Count; i++)
            {
                if (transaction[i][0] == numeroTrans)
                {
                    indexTrans = i;
                    numeroCompte = transaction[i][2];
                    break;
                }
            }


            // Parcourir la liste en sens inverse pour trouver toutes les transactions liées au numero de compte
            for (int i = indexTrans; i >= 0; i--)
            {
                if (transaction[i][2] == numeroCompte)
                {
                    cumulRetrait += transaction[i][1];
                    compteurRetrait++;
                }
                if (compteurRetrait >= 10)
                {
                    break;
                }
            }
            if (cumulRetrait <= 1000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Vérification si un virement est possible.
        public bool VirementEstPossible(int numeroTrans)
        {
            return RetraitEstPossible(numeroTrans);
        }


        // Vérification si un prélèvement est possible.

        public bool PrelevementEstPossible(int numeroTrans)
        {
            return RetraitEstPossible(numeroTrans);
        }

        // Test du statut de la transaction et ecriture du fichier de sortie.
        public void StatutTransaction(string sttsPath)
        {
            transactionFait = new List<int>();
            using (StreamWriter writer = new StreamWriter(sttsPath))
                foreach (var item in transaction)
                {
                    int numeroTrans = item[0];
                    if (!transactionFait.Contains(numeroTrans))
                    {
                        int numeroCompteExp = item[2];
                        int numeroCompteDesti = item[3];
                        decimal soldeCompteExp = decimal.MinValue;
                        decimal soldeCompteDesti = decimal.MinValue;


                        // Affectation des soldes aux compte expéditeur et destinataire.
                        if (compte.ContainsKey(numeroCompteExp))
                        {
                            soldeCompteExp = compte[numeroCompteExp];
                        }
                        if (compte.ContainsKey(numeroCompteDesti))
                        {
                            soldeCompteDesti = compte[numeroCompteDesti];
                        }
                        // Vérification pour un dépôt.

                        if (item[2] == 0 && item[3] != 0)
                        {
                            if (!DepotEstPossible(numeroTrans))
                            {
                                writer.WriteLine($"{numeroTrans};KO");
                            }
                            else
                            {
                                writer.WriteLine($"{numeroTrans};OK");
                                soldeCompteDesti += item[1];
                            }
                        }

                        // Vérification pour un retrait.
                        if (item[2] != 0 && item[3] == 0)
                        {
                            if (!RetraitEstPossible(numeroTrans))
                            {
                                writer.WriteLine($"{numeroTrans};KO");
                            }
                            else
                            {
                                writer.WriteLine($"{numeroTrans};OK");
                                soldeCompteExp -= item[1];
                            }

                        }

                        // Vérification pour un virement/prélèvement.

                        if (item[2] != 0 && item[3] != 0)
                        {
                            if (DepotEstPossible(numeroTrans) && RetraitEstPossible(numeroTrans) && RetraitMaxCumule(numeroTrans))
                            {
                                writer.WriteLine($"{numeroTrans};OK");
                                soldeCompteDesti += item[1];
                                soldeCompteExp -= item[1];
                            }
                            else
                            {
                                writer.WriteLine($"{numeroTrans};KO");
                            }
                        }
                        if (item[2] == 0 && item[3] == 0)
                        {
                            writer.WriteLine($"{numeroTrans};KO");
                        }
                    }
                    else
                    {
                        writer.WriteLine($"{numeroTrans};KO");
                    }

                    transactionFait.Add(numeroTrans);

                }
        }
    }



}
