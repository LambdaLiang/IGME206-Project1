
class Program
{
    static void Main()
    {
        // variables to use
        int playerID = 1;
        bool setupPhase;
        string[] shipArr = ["Carrier", "Battleship", "Destroyer", "Submarine", "Patrol"];
        char[] shipIcon = ['C', 'B', 'D', 'S', 'P'];
        int[] shipLength = [5, 4, 3, 3, 2];
        int currShipIdx = 0;

        // Map dimensions
        int rowDim = 10;
        int colDim = 10;

        // Set up the player's ship board and shooting board
        char[,] player1Ships = new char[rowDim, colDim];
        char[,] player2Ships = new char[rowDim, colDim];
        char[,] player1Shots = new char[rowDim, colDim];
        char[,] player2Shots = new char[rowDim, colDim];


        // Entrance Screen
        // Entrance();

        // Setup Phase
        setupPhase = true;

        SetupText(playerID);

        InitializeBoard(rowDim, colDim, ref player1Ships);
        InitializeBoard(rowDim, colDim, ref player2Ships);
        InitializeBoard(rowDim, colDim, ref player1Shots);
        InitializeBoard(rowDim, colDim, ref player2Shots);


        // Main gameplay loop
        while (true)
        {
            if (setupPhase)
            {
                Console.WriteLine($"Player {playerID}, place your {shipArr[currShipIdx]}.");
            }
            else
            {
                Console.WriteLine($"Player {playerID}, enter your shot coordinates.");
            }
            string? playerInput = Console.ReadLine();
            if (playerInput != null)
            {
                playerInput = playerInput.Trim().ToLower();
            }
            switch (playerInput)
            {
                case "x":
                    {
                        Console.WriteLine("Exit game.");
                        return;
                    }
                case "b":
                    {
                        // Test message
                        Console.WriteLine("Show board.");
                        // Show the current player's own board
                        // DisplayMapFunction();
                        break;
                    }
                case "i":
                    {
                        if (setupPhase)
                        {
                            // Test message
                            Console.WriteLine("Show instructions.");
                            // Show instructions of game only in setup phase
                            // DisplayInstructions(); 
                        }
                        else
                        {
                            // can show instruction at any time
                            Console.WriteLine("Instructions not available during game play.");
                        }
                        break;
                    }
                case "m":
                    {
                        if (!setupPhase)
                        {
                            // Test message
                            Console.WriteLine("Showing shot map.");

                            // Display shot map only in game phase
                            if (playerID == 1)
                            {
                                DisplayShotMap(player1Shots, rowDim, colDim, ref player1Ships, ref player2Ships);
                            }
                            else
                            {
                                DisplayShotMap(player2Shots, rowDim, colDim, ref player2Ships, ref player1Ships);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Map not available during setup.");
                        }
                        break;
                    }
                default:
                    {
                        // Try to parse player input into different token parts, separated by empty space
                        // If not, then prompt "Incorrect Input"
                        // The switch statement only checks for invalid input, not ship out of bound or overlap
                        // That functionality belongs to the ship placement function
                        string[] inputTokens = playerInput.Split(' ');
                        if (setupPhase)
                        {
                            int row;
                            int col;
                            if (inputTokens.Length != 3)
                            {
                                Console.WriteLine("Incorrect input.");
                                break;
                            }
                            bool rowCheck = int.TryParse(inputTokens[0], out row);
                            bool colCheck = int.TryParse(inputTokens[1], out col);
                            string dir = inputTokens[2];

                            if (!rowCheck || !colCheck || ((dir != "h") && (dir != "v")))
                            {
                                Console.WriteLine("Incorrect input.");
                                break;
                            }

                            // Convert user input into array index
                            inputIndexConversion(ref row);
                            inputIndexConversion(ref col);


                            // Try to place ship in order of the ship array
                            Console.WriteLine($"Attempt to place {shipArr[currShipIdx]} on your board.");
                            char[,] playerShipArrayPtr = playerID == 1 ? player1Ships : player2Ships;
                            if (PlaceShipFunction(ref playerShipArrayPtr,
                                row, col, dir, shipLength[currShipIdx], shipIcon[currShipIdx], rowDim, colDim))
                            {
                                Console.WriteLine($"{shipArr[currShipIdx]} successfully placed at {row + 1}:{col + 1}.");
                                currShipIdx += 1;
                                if (currShipIdx == shipArr.Length)
                                {
                                    // Current player has finished set up
                                    Console.WriteLine("Current player set up phase finished.");
                                    if (playerID == 1)
                                    {
                                        currShipIdx = 0;
                                        TurnProgression(ref playerID);
                                        Console.Clear();
                                        SetupText(playerID);
                                    }
                                    else
                                    {
                                        TurnProgression(ref playerID);
                                        setupPhase = false;
                                        Console.Clear();
                                        GameplayText(playerID);
                                    }
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Invalid placement.");
                                break;
                            }

                        }
                        else
                        {
                            int row;
                            int col;
                            if (inputTokens.Length != 2)
                            {
                                Console.WriteLine("Incorrect input.");
                                break;
                            }
                            bool rowCheck = int.TryParse(inputTokens[0], out row);
                            bool colCheck = int.TryParse(inputTokens[1], out col);
                            if (!rowCheck || !colCheck)
                            {
                                Console.WriteLine("Incorrect input.");
                                break;
                            }

                            Console.WriteLine($"Shooting at position {row}:{col}.");

                            // Places an X at the position the player inputted for each player
                            // Since it's only 1v1 game, this makes sense
                            if (playerID == 1)
                            {
                                player1Shots[row, col] = 'X';
                            }
                            else
                            {
                                player2Shots[row, col] = 'X';
                            }

                            // Convert user input into array index
                            inputIndexConversion(ref row);
                            inputIndexConversion(ref col);

                            // Pass input to shooting function

                            //if (ShootFunction(row, col))
                            //{
                            Console.WriteLine("Valid shot.");
                            // Win();
                            TurnProgression(ref playerID);
                            // Clean the screen for next player
                            Console.Clear();
                            GameplayText(playerID);
                            break;
                            //}
                            // else
                            // {
                            //     Console.WriteLine("Invalid shot.");
                            //     break;
                            // }

                            // Only progress to the next player after a valid shot was launched
                        }
                        break;
                    }
            }
        }
    }

    static void TurnProgression(ref int playerID)
    {
        // Possible TODO:
        // Can make player 1 and 2 into player 0 and 1, and then display it using +1
        playerID = (playerID % 2) + 1;
    }

    static void SetupText(int playerID)
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine($"Player {playerID}, please place your ships. Enter 'x' to exit, 'b' to see the current board layout, or 'i' to see the instructions.");
        Console.WriteLine(new string('-', Console.WindowWidth));
    }

    static void GameplayText(int playerID)
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine($"Player {playerID}. It's your turn. Enter, in order, the row and column you would like to target,'b' to see your ship layout, 'm' to see your shot map, or 'x' to exit, then press <enter>.");
        Console.WriteLine(new string('-', Console.WindowWidth));
    }

    static void InitializeBoard(int rowDim, int colDim, ref char[,] board)
    {
        // Initialize the board to fill with '.', the empty marker
        for (int col = 0; col < colDim; col++)
        {
            for (int row = 0; row < rowDim; row++)
            {
                board[row, col] = '.';
            }
        }
    }

    static void inputIndexConversion(ref int coord)
    {
        // This function is used to turn the 1-based user input into 0-based array index
        // Because the user sees 1-10, but the array uses 0-9
        // We need to unify this before passing user input into the array operations
        coord -= 1;
    }
    /// <summary>
    /// Allows both players to place their ships on the map
    /// </summary>
    /// <param name="shipBoard"></param>
    /// <param name="row">Gets the row input from the player</param>
    /// <param name="col">Gets the column input from the player</param>
    /// <param name="direction">Which direction the ship is place in, either h or v</param>
    /// <param name="shipSize">How big is the ship</param>
    /// <param name="shipIcon">C for Carrier, B for Battleship, etc.</param>
    /// <param name="rowDim">Gets all of the rows</param>
    /// <param name="colDim">Gets all of the columns</param>
    /// <returns></returns>
    public static bool PlaceShipFunction(ref char[,] shipBoard, int row, int col, string direction, int shipSize, char shipIcon, int rowDim, int colDim)
    {
        // check starting coordinates on board
        if (row < 0 || row > rowDim - 1 || col < 0 || col > colDim - 1)
        {
            Console.WriteLine("starting coord out of bound");
            return false;
        }

        // Check entire ship fits
        if (direction == "h")
        {
            if (col + shipSize > colDim)
            {
                Console.WriteLine("horizontal ship out of bound");
                return false;
            }
        }
        else
        {
            if (row + shipSize > rowDim)
            {
                Console.WriteLine("vertical ship out of bound");
                return false;
            }
        }

        // Check if every tile we want to put down the ship is "."
        if (direction == "h")
        {
            for (int i = 0; i < shipSize; i++)
            {
                if (shipBoard[row, col + i] != '.')
                {
                    // This tile is occupied by other ships already
                    Console.WriteLine("Tile occupied.");
                    return false;
                }
            }
        }
        else
        {
            for (int i = 0; i < shipSize; i++)
            {
                if (shipBoard[row + i, col] != '.')
                {
                    Console.WriteLine("Tile occupied.");
                    return false;
                }
            }
        }

        // If the ship coordinate passed all checks, we can place it down on the ship board
        if (direction == "h")
        {
            for (int i = 0; i < shipSize; i++)
            {
                shipBoard[row, col + i] = shipIcon;
            }
        }
        else
        {
            for (int i = 0; i < shipSize; i++)
            {
                shipBoard[row + i, col] = shipIcon;
            }
        }
        PrintMap(shipBoard, rowDim, colDim);
        return true;
    }

    // Debug function
    static void PrintMap(char[,] map, int rowDim, int colDim)
    {
        for (int i = 0; i < rowDim; i++)
        {
            for (int j = 0; j < colDim; j++)
            {
                Console.Write($"{map[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Will check to see where the player shoots on the board
    /// </summary>
    /// <param name="shotFired">Gets the board of the map</param>
    /// <param name="rowDim">Gets the rows of the board</param>
    /// <param name="colDim">Gets the columns of the board</param>
    /// <param name="player1Shots"></param>
    /// <param name="player2Shots"></param>
    public static void DisplayShotMap(char[,] shotsFired, int rowDim, int colDim, ref char[,] player1Shots, ref char[,] player2Shots)
    {
        //Goes through the map and displays X for every shot taken
        //TODO: Need to figure out how to replace an X for the shots that the player took
        for (int i = 0; i < rowDim; i++)
        {
            for (int j = 0; j < colDim; j++)
            {
                if (player1Shots[i, j] == shotsFired[i, j])
                {
                    Console.Write(player1Shots[i, j] = 'X');
                }
            }
            Console.WriteLine();
        }
        PrintMap(shotsFired, rowDim, colDim);
    }
}

// When is the first time I can determine win?
// ship or not from shipmap
// shot or not from shootingmap
// arr of int of how many times of the ship has been shot

