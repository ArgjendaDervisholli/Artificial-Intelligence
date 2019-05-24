using System;
using System.Collections.Generic;
using System.Text;

namespace Detyra2
{
    class Program
    {
        public static List<Kanibali> kanibalatMajtas = new List<Kanibali>();
        public static List<Kanibali> kanibalatDjathtas = new List<Kanibali>();

        static void Main(string[] args)
        {

            KerkimInformuar objIS = new KerkimInformuar();
            kanibalatMajtas.Add(new Kanibali(false));
            kanibalatMajtas.Add(new Kanibali(false));
            kanibalatMajtas.Add(new Kanibali(true));

            Puzzle start = new Puzzle(kanibalatMajtas, kanibalatDjathtas,3,0,1,0);

            objIS.copyNode(start);

            start.Parent = null;
            start.G = 0;
            start.H = objIS.CalculateHeuristics(start);
            start.F = start.G + start.H;

            List<Puzzle> objResult = objIS.AStar(start);

            Console.Write("           Majtas  |  Djathtas\n" +
                          "------------------------------\n" +
                          "Pozita Fillestare             \n" +
                          "Kanibal          3 | 0        \n" +
                          "Misionar         3 | 0        \n" +
                          "Anija            1 | 0        \n" +
                          "------------------------------\n");

            int count = 1, j, hendikepMajtas;
            for (int i = objResult.Count - 1; i >= 0; i--)
            {
                Console.Write("Levizja [" + (count++) + "]");

                if (i == objResult.Count - 1)
                    Console.Write("\t--> [2 K]\n");

                hendikepMajtas = 0;

                for (j = 0; j < objResult[i].kanibaletMajtas.Count; j++)
                {
                    if (objResult[i].kanibaletMajtas[j].hendikept)
                    {
                        hendikepMajtas = 1;
                        break;
                    }
                    else
                        hendikepMajtas = 0;
                }

                if(hendikepMajtas == 1)
                {
                    if(objResult[i].anijaMajtas == 0 && i < objResult.Count - 1)
                    {
                        if ((objResult[i + 1].kanibaletMajtas.Count != objResult[i].kanibaletMajtas.Count) && 
                            (objResult[i + 1].misionaretMajtas != objResult[i].misionaretMajtas))
                        {
                            Console.Write("\t--> [K M]\n");
                        }
                        else if((objResult[i + 1].kanibaletMajtas.Count == objResult[i].kanibaletMajtas.Count) &&
                            (objResult[i + 1].misionaretMajtas != objResult[i].misionaretMajtas))
                        {
                            if (Math.Abs(objResult[i + 1].misionaretMajtas - objResult[i].misionaretMajtas) < 2)
                                Console.Write("\t--> [M]\n");
                            else
                                Console.Write("\t--> [2 M]\n");
                        }
                        else if ((objResult[i + 1].kanibaletMajtas.Count != objResult[i].kanibaletMajtas.Count) &&
                            (objResult[i + 1].misionaretMajtas == objResult[i].misionaretMajtas))
                        {
                            if(Math.Abs(objResult[i + 1].kanibaletMajtas.Count - objResult[i].kanibaletMajtas.Count)<2)
                                Console.Write("\t--> [K]\n");
                            else
                                Console.Write("\t--> [2 K]\n");
                        }
                    }
                    else if (i < objResult.Count - 1)
                    {
                        if ((objResult[i + 1].kanibaletDjathtas.Count != objResult[i].kanibaletDjathtas.Count) &&
                            (objResult[i + 1].misionaretDjathtas != objResult[i].misionaretDjathtas))
                        {
                            Console.Write("\t[K* M] <--\n");
                        }
                        else if ((objResult[i + 1].kanibaletDjathtas.Count == objResult[i].kanibaletDjathtas.Count) &&
                            (objResult[i + 1].misionaretDjathtas != objResult[i].misionaretDjathtas))
                        {
                            if (Math.Abs(objResult[i + 1].misionaretMajtas - objResult[i].misionaretMajtas) < 2)
                                Console.Write("\t[M] <--\n");
                            else
                                Console.Write("\t[2 M] <--\n");
                        }
                        else if ((objResult[i + 1].kanibaletDjathtas.Count != objResult[i].kanibaletDjathtas.Count) &&
                            (objResult[i + 1].misionaretDjathtas == objResult[i].misionaretDjathtas))
                        {
                            if (Math.Abs(objResult[i + 1].kanibaletMajtas.Count - objResult[i].kanibaletMajtas.Count) < 2)
                                Console.Write("\t[K] <--\n");
                            else
                                Console.Write("\t[K K*] <--\n");
                        }
                    }
                    Console.Write("Kanibal          " + objResult[i].kanibaletMajtas.Count + " | " + objResult[i].kanibaletDjathtas.Count + "\n" +
                                  "Misionar         " + objResult[i].misionaretMajtas +      " | " + objResult[i].misionaretDjathtas +      "\n" +
                                  "Anija            " + objResult[i].anijaMajtas +           " | " + objResult[i].anijaDjathtas +           "\n" +
                                  "------------------------------\n");
                }
                
                else
                {
                    if (objResult[i].anijaMajtas == 0 && i < objResult.Count - 1)
                    {
                        if ((objResult[i + 1].kanibaletMajtas.Count != objResult[i].kanibaletMajtas.Count) &&
                            (objResult[i + 1].misionaretMajtas != objResult[i].misionaretMajtas))
                        {
                            Console.Write("\t--> [K* M]\n");
                        }
                        else if ((objResult[i + 1].kanibaletMajtas.Count == objResult[i].kanibaletMajtas.Count) &&
                            (objResult[i + 1].misionaretMajtas != objResult[i].misionaretMajtas))
                        {
                            if (Math.Abs(objResult[i + 1].misionaretMajtas - objResult[i].misionaretMajtas) < 2)
                                Console.Write("\t--> [M]\n");
                            else
                                Console.Write("\t--> [2 M]\n");
                        }
                        else if ((objResult[i + 1].kanibaletMajtas.Count != objResult[i].kanibaletMajtas.Count) &&
                            (objResult[i + 1].misionaretMajtas == objResult[i].misionaretMajtas))
                        {
                            if (Math.Abs(objResult[i + 1].kanibaletMajtas.Count - objResult[i].kanibaletMajtas.Count) < 2)
                                Console.Write("\t--> [K]\n");
                            else
                                Console.Write("\t--> [K K*]\n");
                        }
                    }
                    else if (i < objResult.Count - 1)
                    {
                        if ((objResult[i + 1].kanibaletDjathtas.Count != objResult[i].kanibaletDjathtas.Count) &&
                            (objResult[i + 1].misionaretDjathtas != objResult[i].misionaretDjathtas))
                        {
                            Console.Write("\t[K M] <--\n");
                        }
                        else if ((objResult[i + 1].kanibaletDjathtas.Count == objResult[i].kanibaletDjathtas.Count) &&
                            (objResult[i + 1].misionaretDjathtas != objResult[i].misionaretDjathtas))
                        {
                            if (Math.Abs(objResult[i + 1].misionaretMajtas - objResult[i].misionaretMajtas) < 2)
                                Console.Write("\t[M] <--\n");
                            else
                                Console.Write("\t[2 M] <--\n");
                        }
                        else if ((objResult[i + 1].kanibaletDjathtas.Count != objResult[i].kanibaletDjathtas.Count) &&
                            (objResult[i + 1].misionaretDjathtas == objResult[i].misionaretDjathtas))
                        {
                            if (Math.Abs(objResult[i + 1].kanibaletMajtas.Count - objResult[i].kanibaletMajtas.Count) < 2)
                                Console.Write("\t[K] <--\n");
                            else
                                Console.Write("\t[2 K] <--\n");
                        }
                    }
                    Console.Write("Kanibal          " + objResult[i].kanibaletMajtas.Count + " | " + objResult[i].kanibaletDjathtas.Count + "\n" +
                                  "Misionar         " + objResult[i].misionaretMajtas +      " | " + objResult[i].misionaretDjathtas +      "\n" +
                                  "Anija            " + objResult[i].anijaMajtas +           " | " + objResult[i].anijaDjathtas +           "\n" +
                                  "------------------------------\n");
                }
            }

            Console.ReadKey();
        }
    }
}
