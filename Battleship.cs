
class Program
{
    const int V = 0;
    const int H = 1;
    const int BoatSunk = 0;
    const int BoatFloating = 1;

    static void Main()
    {
        // variables to use
        int playerID = 1;
        bool setupPhase;
        string[] shipArr = ["Carrier", "Battleship", "Destroyer", "Submarine", "Patrol"];
        char[] shipIcon = ['C', 'B', 'D', 'S', 'P'];
        int[] shipLength = [5, 4, 3, 3, 2];
        int currShipIdx = 0;
        int startingRow = 0;
        int startingCol = 0;

        // Map dimensions
        int rowDim = 10;
        int colDim = 10;

        // Set up the player's ship board and shooting board
        char[,] player1Ships = new char[rowDim, colDim];
        char[,] player2Ships = new char[rowDim, colDim];
        char[,] player1Shots = new char[rowDim, colDim];
        char[,] player2Shots = new char[rowDim, colDim];

        // Data for both boats
        // Making a 2D to represent the boats that a player has
        // and the data that the player has about that boat.
        // Each row represents another boat,
        // The following indices represents the data inside each boat
        // Index 0 represents starting row of the boat
        // Index 1 represents starting col of the boat
        // Index 2 represents the length
        // Index 3 represents whether the ship is placed horizontally or vertically
        // Index 4 represents whether or not the ship is sunk
        int[,] boatData1 = new int[5,5];
        int[,] boatData2 = new int[5,5];

        // Enterance Screen
        Enterance();

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
                        if (playerID == 1) // Player 1
                        {
                            Console.WriteLine($"Player {playerID}'s board:");
                            //CurrentBoardLayout(ref player1Ships, rowDim, colDim);
                        }
                        else // Player 2
                        {
                            Console.WriteLine($"Player {playerID}'s board:");
                            //CurrentBoardLayout(ref player2Ships, rowDim, colDim);
                        }
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
                                PrintShotMap(player1Shots, rowDim, colDim);
                            }
                            else
                            {
                                PrintShotMap(player2Shots, rowDim, colDim);
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

                            if (playerID == 1)
                            {
                                // Placing all of the data into boatData for Player 1
                                boatData1[currShipIdx, 0] = row;
                                boatData1[currShipIdx, 1] = col;
                                boatData1[currShipIdx, 2] = shipLength[currShipIdx];
                                if (dir == "h")
                                {
                                    // The boat is horizontal
                                    boatData1[currShipIdx, 3] = H;
                                }
                                else if (dir == "v")
                                {
                                    // The boat is NOT horizontal, it's vertical
                                    boatData1[currShipIdx, 3] = V;
                                }
                                boatData1[currShipIdx, 4] = BoatFloating;
                            }
                            else
                            { // Placing all of the data into boatData for Player 2
                                boatData2[currShipIdx, 0] = row;
                                boatData2[currShipIdx, 1] = col;
                                boatData2[currShipIdx, 2] = shipLength[currShipIdx];
                                
                                if (dir == "h")
                                {
                                    boatData2[currShipIdx, 3] = H;
                                }
                                else if (dir == "v")
                                {
                                    boatData2[currShipIdx, 3] = V;
                                }
                                boatData2[currShipIdx, 4] = BoatFloating;
                            }

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

                            // Convert user input into array index
                            inputIndexConversion(ref row);
                            inputIndexConversion(ref col);

                            // Places an X at the position the player inputted for each player
                            // Since it's only 1v1 game, this makes sense
                            if (ShootFunction(row, col))
                            {
                                if (playerID == 1)
                                {
                                    player1Shots[row, col] = 'X';
                                    CheckShots(rowDim, colDim, ref player1Shots, ref player2Ships);
                                }
                                else
                                {
                                    player2Shots[row, col] = 'X';
                                    CheckShots(rowDim, colDim, ref player2Shots, ref player1Ships);
                                }
                            }

                            // If the player sinks another's ship
                            if (playerID == 1) // Player 1 
                            {
                                ShipDefeated(rowDim, colDim, ref shipLength, ref boatData1, player1Shots, player2Ships);
                                Console.WriteLine($"You sunk a ship!");
                            }
                            else // Player 2
                            {
                                //ShipDefeated(rowDim, colDim, ref shipLength, player1Ships);
                                Console.WriteLine($"You sunk a ship!");
                            }

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

    // Welcoming the user to the game
    static void Enterance()
    {
        // Greeting user
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("Welcome to Battleship!\n");
        Console.ResetColor();
        Console.WriteLine("Command your fleets in this classic two-player strategy game of naval combat!\n");
        Console.WriteLine("Each player commands a fleet of 5 ships placed on a 10x10 grid. " +
        "Your goal is to locate and sink all of your opponent's ships before they sink yours!\n");

        // Fleet
        Console.WriteLine("Your Fleet: \n" +
        "1. Aircraft Carrier (Length: 5) (Icon: C)\n" +
        "2. Battleship (Length: 4) (Icon: B)\n" +
        "3. Destroyer (Length: 3) (Icon: D)\n" +
        "4. Submarine (Length: 2) (Icon: S)\n" +
        "5. Patrol (Length: 2) (Icon: P)\n\n");

        // Directions
        Console.WriteLine("1. Setup Phase: " +
        "Both players will take turns placing their 5 ships horizontally “h” or vertically “v” on a 10x10 grid.\n " +
        "\tShips must fit entirely on the board and cannot overlap or be placed diagonally.\n" +
        "2. Game Phase: " +
        "Players take turns firing shots by entering grid coordinates row and column.\n" +
        "3. Tracking Progress: " +
        "On your turn, you can view your own board and shot map to see your hits and misses against the opponent.\n" +
        "4. Winning: " +
        "The first player to successfully sink all 5 enemy ships wins the battle!\n\n");

        // Allowing the user to press a key to begin gameplay
        Console.WriteLine("Press any key to begin playing.");
        Console.WriteLine();
        ConsoleKeyInfo userInput = Console.ReadKey();

        Console.Clear();
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

    public static bool ShipDefeated(int rowDim, int colDim, ref int[] shipLength, ref int[,] boatData, char[,] currentPlayerShootingShips, char[,] opponentShips)
    {
        // Looping through the grid
        for (int row = 0; row < rowDim; row++)
        {
            for (int col = 0; col < colDim; col++)
            {
                // If there is an empty in the opponent's ships and there is an X on the current player's map
                if ((opponentShips[row, col] != '\0') && (currentPlayerShootingShips[row, col] == 'X'))
                {
                    //If the boat is still floating
                    if (opponentShips[row, col] == BoatFloating)
                    {
                        if (boatData[row, col] == opponentShips[row, col])
                        {
                            Console.Write("Hello");
                        }
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Gets the current board of each player
    /// </summary>
    /// <param name="board">Gets the board of each player</param>
    /// <param name="rowDim">Gets the rows of the map</param>
    /// <param name="colDim">Gets the columns of the map</param>
    static void CurrentBoardLayout(ref char[,] board, int rowDim, int colDim)
    {
        for (int i = 0; i < colDim; i++)
        {
            for (int j = 0; j < rowDim; j++)
            {
                Console.Write($"{board[i, j]} ");
            }
            Console.WriteLine();
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
    /// This tells the user if there shot hit inside of the grid
    /// </summary>
    /// <param name="row">Gets the user's y value for input</param>
    /// <param name="col">Gets the user's x value for input</param>
    public static bool ShootFunction(int row, int col)
    {
        //Checking
        if (row > 10 || col > 10)
        {
            Console.WriteLine("Invalid shot");
            return false;
        }
        else
        {
            Console.WriteLine("Valid shot");
            return true;
        }

    }

    /// <summary>
    /// Allows both players to place their ships on the map
    /// </summary>
    /// <param name="shipBoard">Gets a reference of the ship board</param>
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
        PrintShotMap(shipBoard, rowDim, colDim);
        return true;
    }

    /// <summary>
    /// Prints out the shot map
    /// </summary>
    /// <param name="map">Gets the map of each player's shots</param>
    /// <param name="rowDim">Gets the amount of rows</param>
    /// <param name="colDim">Gets the amount of columns</param>
    static void PrintShotMap(char[,] map, int rowDim, int colDim)
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
    /// <param name="rowDim">Gets the rows of the board</param>
    /// <param name="colDim">Gets the columns of the board</param>
    /// <param name="currentPlayerShots">Gets the shots of the current player</param>
    /// <param name="currentOpponentShips">Gets the shots of the opponent player</param>
    public static void CheckShots(int rowDim, int colDim, ref char[,] currentPlayerShots, ref char[,] currentOpponentShips)
    {
        // Tells the player whether or not they made direct contact when shooting at a ship
        for (int i = 0; i < rowDim; i++)
        {
            for (int j = 0; j < colDim; j++)
            {
                // Gets the player1shots and if they equal the player2's ships locations, output something
                // If there is an X at the current players board and if something is there
                if (currentPlayerShots[i, j] == 'X')
                {
                    if (currentOpponentShips[i, j] != '.')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You hit the enemy ship!");
                        ConsoleKeyInfo key = Console.ReadKey();
                        Console.ResetColor();
                        return;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }


}

// When is the first time I can determine win?
// ship or not from shipmap
// shot or not from shootingmap
// arr of int of how many times of the ship has been shot

