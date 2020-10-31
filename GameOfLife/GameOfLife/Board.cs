using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    public class Board
    {
        private int Width = 20;
        private int Height = 20;
        private int[,] fields; //x - height, y - width
        private int[,] fieldsLifeCounter;
        public static readonly int EMPTY_FIELD = 0;
        public static readonly int ALIVE_FIELD = 1;
        public static readonly int GOING_TO_DIE = 2;
        public static readonly int JUST_BORN = 3;

        public int lifeParameter = 0;
        public int neighborsToStayAliveParameter = 0;
        public int neighborsToBeBornParameter = 3;


        private static readonly string START_FILE_PATH =
            @"C:\Users\zmddd\Desktop\Studia\Semestr VII\PLANET\Game-Of-Life\GameOfLife\GameOfLife\Resources\StartBoard.txt";

        private static readonly string SAVE_FILE_PATH =
            @"C:\Users\zmddd\Desktop\Studia\Semestr VII\PLANET\Game-Of-Life\GameOfLife\GameOfLife\Resources\save.txt";

        public Board()
        {
            PrepareBoard(START_FILE_PATH);
        }

        private void PrepareBoard(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    SetHeight(Int32.Parse(sr.ReadLine()));
                    SetWidth(Int32.Parse(sr.ReadLine()));

                    fields = new int[Height, Width];
                    fieldsLifeCounter = new int[Height, Width];
                    for (var i = 0; i < GetHeight(); i++)
                    {
                        string row = sr.ReadLine();
                        for (var j = 0; j < row.Length; j++)
                        {
                            fields[i, j] = (int) Char.GetNumericValue(row[j]);
                            fieldsLifeCounter[i, j] = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        public void LoadBoard()
        {
            PrepareBoard(SAVE_FILE_PATH);
        }

        public void SaveBoard()
        {
            using (StreamWriter sw = new StreamWriter(SAVE_FILE_PATH))
            {
                String line;
                line = Height.ToString();
                sw.WriteLine(line);
                line = Width.ToString();
                sw.WriteLine(line);
                for (var i = 0; i < GetHeight(); i++)
                {
                    line = "";
                    for (var j = 0; j < GetWidth(); j++)
                    {
                        line += fields[i, j].ToString();
                    }

                    sw.WriteLine(line);
                }
            }
        }

        public void NextTurn()
        {
            int[,] newFields = new int[Height, Width];
            for (var i = 0; i < GetHeight(); i++)
            {
                for (var j = 0; j < GetWidth(); j++)
                {
                    newFields[i, j] = fields[i, j];
                }
            }

            for (var i = 0; i < GetHeight(); i++)
            {
                for (var j = 0; j < GetWidth(); j++)
                {
                    int neighbors = CountNeighbors(i, j, fields);
                    if (neighbors == neighborsToBeBornParameter && !isAlive(fields[i, j])) newFields[i, j] = ALIVE_FIELD;
                    else if (!neighborsToStayAlive(neighbors) && isAlive(fields[i, j]))
                        newFields[i, j] = EMPTY_FIELD;
                }
            }

            for (var i = 0; i < GetHeight(); i++)
            {
                for (var j = 0; j < GetWidth(); j++)
                {
                    int neighbors = CountNeighbors(i, j, newFields);
                    if (!isAlive(fields[i, j]) && newFields[i, j] == ALIVE_FIELD) newFields[i, j] = JUST_BORN;
                    else if (isAlive(fields[i, j]) && isAlive(newFields[i, j])) newFields[i, j] = ALIVE_FIELD;

                    if (!neighborsToStayAlive(neighbors) && isAlive(newFields[i, j])) newFields[i, j] = GOING_TO_DIE;

                    //life counting
                    if (lifeParameter != 0)
                    {
                        if (isAlive(newFields[i, j]))
                        {
                            fieldsLifeCounter[i, j] = ++fieldsLifeCounter[i, j];
                            if (fieldsLifeCounter[i, j] == lifeParameter)
                            {
                                newFields[i, j] = EMPTY_FIELD;
                                fieldsLifeCounter[i, j] = 0;
                            }
                        }
                        else
                        {
                            fieldsLifeCounter[i, j] = 0;
                        }
                    }
                }
            }

            for (var i = 0; i < GetHeight(); i++)
            {
                for (var j = 0; j < GetWidth(); j++)
                {
                    //life counting
                    if (lifeParameter != 0)
                    {
                        if (isAlive(newFields[i, j]))
                        {
                            fieldsLifeCounter[i, j] = ++fieldsLifeCounter[i, j];
                            if (fieldsLifeCounter[i, j] == lifeParameter)
                            {
                                newFields[i, j] = EMPTY_FIELD;
                                fieldsLifeCounter[i, j] = 0;
                            }
                        }
                        else
                        {
                            fieldsLifeCounter[i, j] = 0;
                        }
                    }
                }
            }

            fields = newFields;
        }

        public bool neighborsToStayAlive(int neighbors)
        {
            if (neighborsToStayAliveParameter != 0)
            {
                return neighbors == neighborsToStayAliveParameter;
            }
            else
            {
                return (neighbors == 2 || neighbors == 3);
            }
        }

        public void SetWidth(int width)
        {
            Width = width;
        }

        public int GetWidth()
        {
            return Width;
        }

        public void SetHeight(int height)
        {
            Height = height;
        }

        public int GetHeight()
        {
            return Height;
        }

        public void SetLife(int life)
        {
            lifeParameter = life;
        }

        public void SetNeighborsToStayAlive(int param)
        {
            neighborsToStayAliveParameter = param;
        }

        public void SetNeighborsToBeBorn(int param)
        {
            neighborsToBeBornParameter = param;
        }

        public SolidColorBrush GetFieldColor(int x, int y)
        {
            int field = fields[x, y];
            if (field == ALIVE_FIELD) return Brushes.GreenYellow;
            else if (field == JUST_BORN) return Brushes.DarkOrange;
            else if (field == GOING_TO_DIE) return Brushes.Black;
            else return Brushes.DarkCyan;
        }

        public void setField(int x, int y, int field)
        {
            fields[x, y] = field;
        }

        public int GetEmptyField()
        {
            return EMPTY_FIELD;
        }

        public int GetAliveField()
        {
            return ALIVE_FIELD;
        }

        private int CountNeighbors(int x, int y, int [,] fieldsToCheck)
        {
            int counter = 0;
            if (x - 1 >= 0 && y - 1 >= 0) //high left
            {
                if (isAlive(fieldsToCheck[x - 1, y - 1])) counter++;
            }

            if (x - 1 >= 0) //high
            {
                if (isAlive(fieldsToCheck[x - 1, y])) counter++;
            }

            if (y - 1 >= 0) //left
            {
                if (isAlive(fieldsToCheck[x, y - 1])) counter++;
            }

            if (x - 1 >= 0 && y + 1 < Width) //high right
            {
                if (isAlive(fieldsToCheck[x - 1, y + 1])) counter++;
            }

            if (x + 1 < Height && y - 1 >= 0) //down left
            {
                if (isAlive(fieldsToCheck[x + 1, y - 1])) counter++;
            }

            if (x + 1 < Height && y + 1 < Width) //down right
            {
                if (isAlive(fieldsToCheck[x + 1, y + 1])) counter++;
            }

            if (x + 1 < Height) //down
            {
                if (isAlive(fieldsToCheck[x + 1, y])) counter++;
            }

            if (y + 1 < Width) //down right
            {
                if (isAlive(fieldsToCheck[x, y + 1])) counter++;
            }

            return counter;
        }

        public bool isAlive(int field)
        {
            return field == ALIVE_FIELD || field == GOING_TO_DIE || field == JUST_BORN;
        }

        public void resizeBoard(int x, int y)
        {
            int[,] oldFields = new int[Height, Width];
            for (var i = 0; i < GetHeight(); i++)
            {
                for (var j = 0; j < GetWidth(); j++)
                {
                    oldFields[i, j] = fields[i, j];
                }
            }

            int oldHeight = Height;
            int oldWidth = Width;
            Height = x;
            Width = y;
            fields = new int[Height, Width];
            fieldsLifeCounter = new int[Height, Width];

            for (var i = 0; i < GetHeight(); i++)
            {
                for (var j = 0; j < GetWidth(); j++)
                {
                    if (i < oldHeight && j < oldWidth)
                    {
                        fields[i, j] = oldFields[i, j];
                    }
                    else
                    {
                        fields[i, j] = EMPTY_FIELD;
                    }

                    fieldsLifeCounter[i, j] = 0;
                }
            }
        }



        public void AddFigure(int x, int y, List<Tuple<int, int>> pos)
        {
            foreach (var position in pos)
            {
                SetField(x + position.Item1, y + position.Item2);
            }
        }

        public void SetField(int x, int y)
        {
            if (x >= 0 && x < Height && y >= 0 && y < Width)
            {
                fields[x, y] = JUST_BORN;
            }
        }
    }
}
