using System;
using System.Collections.Generic;
using System.Text;

namespace Detyra2
{
    class KerkimInformuar
    {
        // target Puzzle
        public Puzzle target;

        List<Kanibali> kanibaliLeft = new List<Kanibali>();
        List<Kanibali> kanibaliRight = new List<Kanibali>();
        
        public List<Puzzle> AStar(Puzzle start)
        {
            kanibaliRight.Add(new Kanibali(false));
            kanibaliRight.Add(new Kanibali(false));
            kanibaliRight.Add(new Kanibali(true));

            target = new Puzzle(kanibaliLeft, kanibaliRight, 0, 3, 0, 1);

            List<Puzzle> PathToSolution = new List<Puzzle>();
            List<Puzzle> OpenList = new List<Puzzle>();
            List<Puzzle> ClosedList = new List<Puzzle>();

            OpenList.Add(start);
            bool goalFound = false;
            Puzzle currentPuzzle = null;

            while (OpenList.Count > 0 && !goalFound)
            {
                // zgjedh node me F score me te vogel
                currentPuzzle = lowestFScoreLocation(OpenList);

                if (CheckIfTarget(currentPuzzle))
                {
                    goalFound = true;
                    continue;
                }

                ClosedList.Add(currentPuzzle);
                OpenList.Remove(currentPuzzle);

                List<Puzzle> neighbours = GenerateNeighbours(currentPuzzle);

                for (int i = 0; i < neighbours.Count; i++)
                {
                    Puzzle currentNeighbour = neighbours[i];
                    //nese eshte ne closed list continue
                    if (Exists(currentNeighbour, ClosedList))
                        continue;

                    if (!Exists(currentNeighbour, OpenList))
                        OpenList.Add(currentNeighbour);
                }
            }

            if (goalFound)
            {
                while (currentPuzzle.Parent != null)
                {
                    PathToSolution.Add(currentPuzzle);
                    currentPuzzle = currentPuzzle.Parent;
                }
            }

            return PathToSolution;
        }

        public Puzzle lowestFScoreLocation(List<Puzzle> ol)
        {
            int lowest = ol[0].F;
            Puzzle lowestLoc = ol[0];
            for (int i = 0; i < ol.Count; i++)
            {
                if (ol[i].F < lowest)
                {
                    lowest = ol[i].F;
                    lowestLoc = ol[i];
                }
            }
            return lowestLoc;
        }

        public bool CheckIfTarget(Puzzle _currentPuzzle)
        {
            if (_currentPuzzle.kanibaletDjathtas.Count == target.kanibaletDjathtas.Count &&
                _currentPuzzle.kanibaletMajtas.Count == target.kanibaletMajtas.Count &&

                _currentPuzzle.misionaretDjathtas == target.misionaretDjathtas &&
                _currentPuzzle.misionaretMajtas == target.misionaretMajtas &&

                _currentPuzzle.anijaDjathtas == target.anijaDjathtas &&
                _currentPuzzle.anijaMajtas == target.anijaMajtas)
                return true;

            return false;
        }

        public List<Puzzle> GenerateNeighbours(Puzzle _currentPuzzle)
        {
            List<Puzzle> neighbours = new List<Puzzle>();

            // anija -> majtas
            if (_currentPuzzle.anijaMajtas == 1)
            {
                /********************************************************************************
                 *              1. 1 Kanibal
                 *                   1.1 remove Kanibal where kanibal.hendikept=false
                 ********************************************************************************/
                if (_currentPuzzle.kanibaletMajtas.Count > 0)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count - 1) <= _currentPuzzle.misionaretMajtas ||
                        _currentPuzzle.misionaretMajtas == 0)
                        &&
                        ((_currentPuzzle.kanibaletDjathtas.Count + 1) <= _currentPuzzle.misionaretDjathtas ||
                        _currentPuzzle.misionaretDjathtas == 0))
                    {
                        if (hendikeptExists(_currentPuzzle.kanibaletMajtas, false, 1))
                        {
                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletMajtas, false, 1);
                            _neighbour.kanibaletDjathtas.Add(new Kanibali(false));
                            _neighbour.anijaDjathtas = 1;
                            _neighbour.anijaMajtas = 0;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }

                /********************************************************************************
                *              2. 2 Kanibala
                *                   2.1 remove 2 Kanibals where kanibal.hendikept=false;
                ********************************************************************************/
                if (_currentPuzzle.kanibaletMajtas.Count >1)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count - 2) <= _currentPuzzle.misionaretMajtas ||
                     _currentPuzzle.misionaretMajtas == 0)
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count + 2) <= _currentPuzzle.misionaretDjathtas ||
                     _currentPuzzle.misionaretDjathtas == 0))
                    {

                        if (hendikeptExists(_currentPuzzle.kanibaletMajtas, false, 2))
                        {
                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletMajtas, false, 2);
                            _neighbour.kanibaletDjathtas.Add(new Kanibali(false));
                            _neighbour.kanibaletDjathtas.Add(new Kanibali(false));
                            _neighbour.anijaDjathtas = 1;
                            _neighbour.anijaMajtas = 0;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }

                /********************************************************************************
                 *              3. 1 Misionar 
                 *                   3.1 subtract -1 Misionare
                 ********************************************************************************/
                if (_currentPuzzle.misionaretMajtas > 0)
                {
                    if ((_currentPuzzle.kanibaletMajtas.Count <= (_currentPuzzle.misionaretMajtas - 1) ||
                     (_currentPuzzle.misionaretMajtas - 1) == 0)
                    &&
                    (_currentPuzzle.kanibaletDjathtas.Count <= (_currentPuzzle.misionaretDjathtas + 1)))
                    {
                        Puzzle _neighbour = copyNode(_currentPuzzle);

                        _neighbour.misionaretMajtas = _neighbour.misionaretMajtas - 1;
                        _neighbour.misionaretDjathtas = _neighbour.misionaretDjathtas + 1;
                        _neighbour.anijaDjathtas = 1;
                        _neighbour.anijaMajtas = 0;

                        _neighbour.G = _currentPuzzle.G + 1;
                        _neighbour.H = CalculateHeuristics(_neighbour);
                        _neighbour.F = _neighbour.G + _neighbour.H;
                        _neighbour.Parent = _currentPuzzle;

                        neighbours.Add(_neighbour);
                    }
                }

                /********************************************************************************
                *              4. 2 Misionar 
                *                 4.1  subtract - 2 Misionare
                ********************************************************************************/
                if (_currentPuzzle.misionaretMajtas > 1)
                {
                    if ((_currentPuzzle.kanibaletMajtas.Count <= (_currentPuzzle.misionaretMajtas - 2) ||
                     (_currentPuzzle.misionaretMajtas - 2) == 0)
                    &&
                    (_currentPuzzle.kanibaletDjathtas.Count <= (_currentPuzzle.misionaretDjathtas + 2)))
                    {
                        Puzzle _neighbour = copyNode(_currentPuzzle);

                        _neighbour.misionaretMajtas = _neighbour.misionaretMajtas - 2;
                        _neighbour.misionaretDjathtas = _neighbour.misionaretDjathtas + 2;
                        _neighbour.anijaDjathtas = 1;
                        _neighbour.anijaMajtas = 0;

                        _neighbour.G = _currentPuzzle.G + 1;
                        _neighbour.H = CalculateHeuristics(_neighbour);
                        _neighbour.F = _neighbour.G + _neighbour.H;
                        _neighbour.Parent = _currentPuzzle;

                        neighbours.Add(_neighbour);
                    }
                }

                /********************************************************************************
                 *              5. 1 Kanibal & 1 Misionar 
                 *                     5.1 subtract 1 Kanibal where kanibal.hendikept=false &
                 *                     subtract 1 Misionare
                 ********************************************************************************/
                if (_currentPuzzle.kanibaletMajtas.Count > 0 && _currentPuzzle.misionaretMajtas > 0)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count - 1) <= (_currentPuzzle.misionaretMajtas - 1) ||
                     (_currentPuzzle.misionaretMajtas - 1) == 0)
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count + 1) <= (_currentPuzzle.misionaretDjathtas + 1)))
                    {

                        if (hendikeptExists(_currentPuzzle.kanibaletMajtas, false, 1))
                        {
                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletMajtas, false, 1);
                            _neighbour.kanibaletDjathtas.Add(new Kanibali(false));

                            _neighbour.misionaretMajtas = _neighbour.misionaretMajtas - 1;
                            _neighbour.misionaretDjathtas = _neighbour.misionaretDjathtas + 1;

                            _neighbour.anijaDjathtas = 1;
                            _neighbour.anijaMajtas = 0;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }

                /********************************************************************************
                *              6. 1 Kanibal & 1 Kanibal hendikept
                *                     6.1 remove Kanibal where kanibal.hendikept=false & 
                *                     remove Kanibal where kanibal.hendikept=true
                ********************************************************************************/
                if (_currentPuzzle.kanibaletMajtas.Count > 1)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count - 2) <= _currentPuzzle.misionaretMajtas ||
                     _currentPuzzle.misionaretMajtas == 0)
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count + 2) <= _currentPuzzle.misionaretDjathtas ||
                    _currentPuzzle.misionaretDjathtas == 0))
                    {
                        if (hendikeptExists(_currentPuzzle.kanibaletMajtas, true, 1))
                        {
                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletMajtas, false, 1);
                            removeKanibal(_neighbour.kanibaletMajtas, true, 1);
                            _neighbour.kanibaletDjathtas.Add(new Kanibali(false));
                            _neighbour.kanibaletDjathtas.Add(new Kanibali(true));
                            _neighbour.anijaDjathtas = 1;
                            _neighbour.anijaMajtas = 0;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }

                /********************************************************************************
                *             7. 1 Misionar & 1 Kanibal hendikept
                *                   7.1 subtrack -1 Misionare & 
                *                   remove Kanibal where kanibal.hendikept=true 
                ********************************************************************************/
                if (_currentPuzzle.kanibaletMajtas.Count > 0 && _currentPuzzle.misionaretMajtas > 0)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count - 1) <= (_currentPuzzle.misionaretMajtas - 1) ||
                     (_currentPuzzle.misionaretMajtas - 1) == 0)
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count + 1) <= (_currentPuzzle.misionaretDjathtas + 1)))
                    {
                        if (hendikeptExists(_currentPuzzle.kanibaletMajtas, true, 1))
                        {

                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletMajtas, true, 1);
                            _neighbour.kanibaletDjathtas.Add(new Kanibali(true));

                            _neighbour.misionaretMajtas = _neighbour.misionaretMajtas - 1;
                            _neighbour.misionaretDjathtas = _neighbour.misionaretDjathtas + 1;

                            _neighbour.anijaDjathtas = 1;
                            _neighbour.anijaMajtas = 0;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }
            }
            else // anija -> djathtas
            {
                /********************************************************************************
                 *              1. 1 Kanibal
                 *                   1.1 remove Kanibal where kanibal.hendikept=false
                 ********************************************************************************/
                if (_currentPuzzle.kanibaletDjathtas.Count > 0)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count + 1) <= _currentPuzzle.misionaretMajtas ||
                     _currentPuzzle.misionaretMajtas == 0)
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count - 1) <= _currentPuzzle.misionaretDjathtas ||
                    _currentPuzzle.misionaretDjathtas == 0))
                    {
                        if (hendikeptExists(_currentPuzzle.kanibaletDjathtas, false, 1))
                        {
                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletDjathtas, false, 1);
                            _neighbour.kanibaletMajtas.Add(new Kanibali(false));
                            _neighbour.anijaDjathtas = 0;
                            _neighbour.anijaMajtas = 1;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }

                /********************************************************************************
                *              2. 2 Kanibala
                *                   2.1 remove 2 Kanibals where kanibal.hendikept=false;
                ********************************************************************************/
                if (_currentPuzzle.kanibaletDjathtas.Count > 1)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count + 2) <= _currentPuzzle.misionaretMajtas ||
                     _currentPuzzle.misionaretMajtas == 0)
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count - 2) <= _currentPuzzle.misionaretDjathtas ||
                    _currentPuzzle.misionaretDjathtas == 0))
                    {
                        if (hendikeptExists(_currentPuzzle.kanibaletDjathtas, false, 2))
                        {
                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletDjathtas, false, 2);
                            _neighbour.kanibaletMajtas.Add(new Kanibali(false));
                            _neighbour.kanibaletMajtas.Add(new Kanibali(false));
                            _neighbour.anijaDjathtas = 0;
                            _neighbour.anijaMajtas = 1;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }

                /********************************************************************************
                 *              3. 1 Misionar 
                 *                   3.1 subtract -1 Misionare
                 ********************************************************************************/
                if (_currentPuzzle.misionaretDjathtas > 0)
                {
                    if ((_currentPuzzle.kanibaletMajtas.Count <= (_currentPuzzle.misionaretMajtas + 1))
                    &&
                    (_currentPuzzle.kanibaletDjathtas.Count <= (_currentPuzzle.misionaretDjathtas - 1) ||
                    (_currentPuzzle.misionaretDjathtas-1) == 0))
                    {
                        Puzzle _neighbour = copyNode(_currentPuzzle);

                        _neighbour.misionaretDjathtas = _neighbour.misionaretDjathtas - 1;
                        _neighbour.misionaretMajtas = _neighbour.misionaretMajtas + 1;
                        _neighbour.anijaDjathtas = 0;
                        _neighbour.anijaMajtas = 1;

                        _neighbour.G = _currentPuzzle.G + 1;
                        _neighbour.H = CalculateHeuristics(_neighbour);
                        _neighbour.F = _neighbour.G + _neighbour.H;
                        _neighbour.Parent = _currentPuzzle;

                        neighbours.Add(_neighbour);
                    }
                }

                /********************************************************************************
                *              4. 2 Misionar 
                *                 4.1  subtract - 2 Misionare
                ********************************************************************************/
                if (_currentPuzzle.misionaretDjathtas > 1)
                {
                    if ((_currentPuzzle.kanibaletMajtas.Count <= (_currentPuzzle.misionaretMajtas + 2))
                    &&
                    (_currentPuzzle.kanibaletDjathtas.Count <= (_currentPuzzle.misionaretDjathtas - 2) ||
                    (_currentPuzzle.misionaretDjathtas - 2) == 0))
                    {
                        Puzzle _neighbour = copyNode(_currentPuzzle);

                        _neighbour.misionaretDjathtas = _neighbour.misionaretDjathtas - 2;
                        _neighbour.misionaretMajtas = _neighbour.misionaretMajtas + 2;
                        _neighbour.anijaDjathtas = 0;
                        _neighbour.anijaMajtas = 1;

                        _neighbour.G = _currentPuzzle.G + 1;
                        _neighbour.H = CalculateHeuristics(_neighbour);
                        _neighbour.F = _neighbour.G + _neighbour.H;
                        _neighbour.Parent = _currentPuzzle;

                        neighbours.Add(_neighbour);
                    }
                }

                /********************************************************************************
                 *              5. 1 Kanibal & 1 Misionar 
                 *                     5.1 subtract -1 Kanibal where kanibal.hendikept=false &
                 *                     subtract -1 Misionare
                 ********************************************************************************/
                if (_currentPuzzle.kanibaletDjathtas.Count>0 && _currentPuzzle.misionaretDjathtas > 0)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count + 1) <= (_currentPuzzle.misionaretMajtas + 1))
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count - 1) <= (_currentPuzzle.misionaretDjathtas - 1) ||
                    (_currentPuzzle.misionaretDjathtas - 1) == 0))
                    {
                        if (hendikeptExists(_currentPuzzle.kanibaletDjathtas, false, 1))
                        {
                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletDjathtas, false, 1);
                            _neighbour.kanibaletMajtas.Add(new Kanibali(false));

                            _neighbour.misionaretDjathtas = _neighbour.misionaretDjathtas - 1;
                            _neighbour.misionaretMajtas = _neighbour.misionaretMajtas + 1;

                            _neighbour.anijaDjathtas = 0;
                            _neighbour.anijaMajtas = 1;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }

                /********************************************************************************
                *              6. 1 Kanibal & 1 Kanibal hendikept
                *                     6.1 remove Kanibal where kanibal.hendikept=false & 
                *                     remove Kanibal where kanibal.hendikept=true
                ********************************************************************************/
                if (_currentPuzzle.kanibaletDjathtas.Count > 1)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count + 2) <= _currentPuzzle.misionaretMajtas ||
                     _currentPuzzle.misionaretMajtas == 0)
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count - 2) <= _currentPuzzle.misionaretDjathtas ||
                    _currentPuzzle.misionaretDjathtas == 0))
                    {
                        if (hendikeptExists(_currentPuzzle.kanibaletDjathtas, true, 1))
                        {
                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletDjathtas, false, 1);
                            removeKanibal(_neighbour.kanibaletDjathtas, true, 1);
                            _neighbour.kanibaletMajtas.Add(new Kanibali(false));
                            _neighbour.kanibaletMajtas.Add(new Kanibali(true));
                            _neighbour.anijaDjathtas = 0;
                            _neighbour.anijaMajtas = 1;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }

                /********************************************************************************
                *             7. 1 Misionar & 1 Kanibal hendikept
                *                   7.1 subtrack -1 Misionare & 
                *                   remove Kanibal where kanibal.hendikept=true 
                ********************************************************************************/
                if (_currentPuzzle.kanibaletDjathtas.Count > 0 && _currentPuzzle.misionaretDjathtas > 0)
                {
                    if (((_currentPuzzle.kanibaletMajtas.Count + 1) <= (_currentPuzzle.misionaretMajtas + 1))
                    &&
                    ((_currentPuzzle.kanibaletDjathtas.Count - 1) <= (_currentPuzzle.misionaretDjathtas - 1) ||
                    (_currentPuzzle.misionaretDjathtas - 1) == 0))
                    {
                        if (hendikeptExists(_currentPuzzle.kanibaletDjathtas, true, 1))
                        {

                            Puzzle _neighbour = copyNode(_currentPuzzle);

                            removeKanibal(_neighbour.kanibaletDjathtas, true, 1);
                            _neighbour.kanibaletMajtas.Add(new Kanibali(true));

                            _neighbour.misionaretDjathtas = _neighbour.misionaretDjathtas - 1;
                            _neighbour.misionaretMajtas = _neighbour.misionaretMajtas + 1;

                            _neighbour.anijaDjathtas = 0;
                            _neighbour.anijaMajtas = 1;

                            _neighbour.G = _currentPuzzle.G + 1;
                            _neighbour.H = CalculateHeuristics(_neighbour);
                            _neighbour.F = _neighbour.G + _neighbour.H;
                            _neighbour.Parent = _currentPuzzle;

                            neighbours.Add(_neighbour);
                        }
                    }
                }
            }

            return neighbours;
        }

        public Boolean hendikeptExists(List<Kanibali> listaKanibali, Boolean typeBoolean, int numberOfKanibals)
        {
            Boolean hendikeptExists = false;
            int k = 0;
                for (int i = 0; i < listaKanibali.Count; i++)
                {
                    if ( listaKanibali[i].hendikept == typeBoolean)
                    {
                        k++;
                        if (k == numberOfKanibals)
                        {
                            hendikeptExists = true;
                            break;
                        }
                    }
                }
            return hendikeptExists;
        }
        
        public Boolean removeKanibal(List<Kanibali> listKanibals, Boolean hendikept, int kanibalsToRemove)
        {
            // sa elemente i kemi fshi deri tani
            int k = 0;
            int size = listKanibals.Count;
            for (int i = 0; i < size; i++)
            {
                if (listKanibals[i-k].hendikept == hendikept)
                {
                    listKanibals.RemoveAt(i - k);
                    k++;
                    if (kanibalsToRemove == k)
                        return true;
                }
            }
            return false;
        }

        public int CalculateHeuristics(Puzzle _currentPuzzle)
        {
            return  (int)(Math.Pow(_currentPuzzle.kanibaletMajtas.Count, 2) +
                                   Math.Pow(_currentPuzzle.misionaretMajtas, 2) +
                                   2 * (_currentPuzzle.kanibaletMajtas.Count * _currentPuzzle.misionaretMajtas));
        }

        public bool Exists(Puzzle _currentLocation, List<Puzzle> _list)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].kanibaletDjathtas.Count == _currentLocation.kanibaletDjathtas.Count &&
                    _list[i].kanibaletMajtas.Count == _currentLocation.kanibaletMajtas.Count &&

                    _list[i].misionaretDjathtas == _currentLocation.misionaretDjathtas &&
                    _list[i].misionaretMajtas == _currentLocation.misionaretMajtas &&

                    _list[i].anijaDjathtas == _currentLocation.anijaDjathtas &&
                    _list[i].anijaMajtas == _currentLocation.anijaMajtas )
                {
                    int trueHendikeptDjathtas = 0;
                    int falseHendikeptDjathtas = 0;
                    int trueHendikeptCurrentDjathtas = 0;
                    int falseHendikeptCurrentDjathtas = 0;

                    int trueHendikeptMajtas = 0;
                    int falseHendikeptMajtas = 0;
                    int trueHendikeptCurrentMajtas = 0;
                    int falseHendikeptCurrentMajtas = 0;
                    // 3 munet me kon numri i kanibalave
                    for (int j = 0; j < 3 ; j++)
                    {
                        if (_list[i].kanibaletDjathtas.Count > j)
                        {
                            if (_list[i].kanibaletDjathtas[j].hendikept)
                                trueHendikeptDjathtas++;
                            else
                                falseHendikeptDjathtas++;

                            if (_currentLocation.kanibaletDjathtas[j].hendikept)
                                trueHendikeptCurrentDjathtas++;
                            else
                                falseHendikeptCurrentDjathtas++;
                        }

                        if (_list[i].kanibaletMajtas.Count > j)
                        {
                            if (_list[i].kanibaletMajtas[j].hendikept)
                                trueHendikeptMajtas++;
                            else
                                falseHendikeptMajtas++;

                            if (_currentLocation.kanibaletMajtas[j].hendikept)
                                trueHendikeptCurrentMajtas++;
                            else
                                falseHendikeptCurrentMajtas++;
                            
                        }
                    }

                    if (trueHendikeptDjathtas != trueHendikeptCurrentDjathtas || 
                        falseHendikeptDjathtas != falseHendikeptCurrentDjathtas)
                    {
                        return false;
                    }

                    if (trueHendikeptMajtas != trueHendikeptCurrentMajtas ||
                        falseHendikeptMajtas != falseHendikeptCurrentMajtas)
                    {
                        return false;
                    }
                    return true;
                }
            }

            return false;
        }

        public Puzzle copyNode(Puzzle _currentPuzzle)
        {
            Puzzle newPuzzle = new Puzzle(null,null,0,0,0,0);

            newPuzzle.kanibaletDjathtas = new List<Kanibali>();
            for (int i = 0; i < _currentPuzzle.kanibaletDjathtas.Count; i++)
            {
                newPuzzle.kanibaletDjathtas.Add(new Kanibali(_currentPuzzle.kanibaletDjathtas[i].hendikept));
            }

            newPuzzle.kanibaletMajtas = new List<Kanibali>();
            for (int i = 0; i < _currentPuzzle.kanibaletMajtas.Count; i++)
            {
                newPuzzle.kanibaletMajtas.Add(new Kanibali(_currentPuzzle.kanibaletMajtas[i].hendikept));
            }

            newPuzzle.anijaDjathtas = _currentPuzzle.anijaDjathtas;
            newPuzzle.anijaMajtas = _currentPuzzle.anijaMajtas;
            newPuzzle.misionaretDjathtas = _currentPuzzle.misionaretDjathtas;
            newPuzzle.misionaretMajtas = _currentPuzzle.misionaretMajtas;

            return newPuzzle;

        }
    }
}
