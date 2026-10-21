
class Program
{
    //const int V = 0;
    //const int H = 1;
    //const int BoatSunk = 0;
    //const int BoatFloating = 1;

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

        // Set up the players' ship status array
        // False means the ship is still floating
        // True means the ship has sunk
        bool[] player1ShipStatus = new bool[5];
        bool[] player2ShipStatus = new bool[5];

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
        int[,] boatData1 = new int[5, 5];
        int[,] boatData2 = new int[5, 5];

        // Enterance Screen
        Enterance();

        // Setup Phase
        setupPhase = true;

        SetupText(playerID);

        // Initializing boards for players 1 & 2 and their shot maps
        InitializeBoard(rowDim, colDim, ref player1Ships);
        InitializeBoard(rowDim, colDim, ref player2Ships);
        InitializeBoard(rowDim, colDim, ref player1Shots);
        InitializeBoard(rowDim, colDim, ref player2Shots);

        // Main gameplay loop
        while (true)
        {
            if (setupPhase)
            {
                Console.WriteLine($"Player {playerID}, place your {shipArr[currShipIdx]} (Length = {shipLength[currShipIdx]}).");
                Console.Write("Enter here >> ");
            }
            else
            {
                Console.WriteLine($"Player {playerID}, enter your shot coordinates.");
                Console.Write("Enter here >> ");
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
                        // Shows the current player's own board
                        if (playerID == 1) // Player 1
                        {
                            Console.WriteLine($"Player {playerID}'s board:");
                            CurrentBoardLayout(ref player1Ships, rowDim, colDim);
                        }
                        else // Player 2
                        {
                            Console.WriteLine($"Player {playerID}'s board:");
                            CurrentBoardLayout(ref player2Ships, rowDim, colDim);
                        }
                        break;
                    }
                case "i":
                    {
                        Console.WriteLine("Showing instructions:");
                        // Shows instructions of game only in setup phase
                        DisplayInstructions(); 
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

                            // if (playerID == 1)
                            // {
                            //     // Placing all of the data into boatData for Player 1
                            //     boatData1[currShipIdx, 0] = row;
                            //     boatData1[currShipIdx, 1] = col;
                            //     boatData1[currShipIdx, 2] = shipLength[currShipIdx];
                            //     if (dir == "h")
                            //     {
                            //         // The boat is horizontal
                            //         boatData1[currShipIdx, 3] = H;
                            //     }
                            //     else if (dir == "v")
                            //     {
                            //         // The boat is NOT horizontal, it's vertical
                            //         boatData1[currShipIdx, 3] = V;
                            //     }
                            //     boatData1[currShipIdx, 4] = BoatFloating;
                            // }
                            // else
                            // { // Placing all of the data into boatData for Player 2
                            //     boatData2[currShipIdx, 0] = row;
                            //     boatData2[currShipIdx, 1] = col;
                            //     boatData2[currShipIdx, 2] = shipLength[currShipIdx];

                            //     if (dir == "h")
                            //     {
                            //         boatData2[currShipIdx, 3] = H;
                            //     }
                            //     else if (dir == "v")
                            //     {
                            //         boatData2[currShipIdx, 3] = V;
                            //     }
                            //     boatData2[currShipIdx, 4] = BoatFloating;
                            // }

                            if (!rowCheck || !colCheck || ((dir != "h") && (dir != "v")))
                            {
                                Console.WriteLine("Incorrect input.");
                                break;
                            }

                            // Convert user input into array index
                            InputIndexConversion(ref row);
                            InputIndexConversion(ref col);


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
                                        // TODO: temporary disable clear
                                        // Console.Clear();
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
                            InputIndexConversion(ref row);
                            InputIndexConversion(ref col);

                            // Determine whose map we are calling
                            // If it's player 1's turn, use player 1's shot map
                            // vice versa
                            char[,] currentPlayerShotPtr = playerID == 1 ? player1Shots : player2Shots;
                            // Determine which ship map we are comparing
                            // If it's player 1's turn, use ***player 2*** ship map
                            char[,] opponentShipMapPtr = playerID == 1 ? player2Ships : player1Ships;
                            // Determine which ShipStatus array we should update
                            // If it's player 2' turn, use ***player 2*** ship status array
                            bool[] opponentPlayerShipStatusPtr = playerID == 1 ? player2ShipStatus : player1ShipStatus;
                            if (ShootFunction(ref currentPlayerShotPtr, ref opponentShipMapPtr, row, col, rowDim, colDim))
                            {
                                string? sankShip = null;
                                CurrentBoardLayout(ref currentPlayerShotPtr, rowDim, colDim);
                                if (currentPlayerShotPtr[row, col] == 'X')
                                {
                                    // Only check for ship sank when it's an actual hit from this shot
                                    if (ShipDefeated(opponentShipMapPtr[row, col], opponentShipMapPtr, currentPlayerShotPtr, rowDim, colDim))
                                    {
                                        sankShip = ShipSankUpdate(opponentShipMapPtr[row, col], ref opponentPlayerShipStatusPtr, shipArr, shipIcon);
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine($"You have sunk the {sankShip} of your opponent!");
                                        Console.ResetColor();
                                    }
                                }
                                // If the player has sunk the entirity of their opponents ships
                                if (WinCheck(opponentPlayerShipStatusPtr))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"Player {playerID} has won the game!");
                                    Console.ResetColor();
                                    return;
                                }
                                TurnProgression(ref playerID);
                                // TODO: temporary disable clear
                                // Console.Clear();
                                GameplayText(playerID);
                                Console.WriteLine("This is where your opponent shoot last turn.");
                                CurrentBoardLayout(ref currentPlayerShotPtr, rowDim, colDim);
                                if (sankShip != null)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine($"Your {sankShip} has been sunk by your opponent!");
                                    Console.ResetColor();
                                }
                            }
                            break;
                        }
                    }
            }
        }
    }

    /// <summary>
    /// Welcoming the user to the game
    /// </summary>
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
        "4. Submarine (Length: 3) (Icon: S)\n" +
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
    /// <summary>
    /// Provides the instructions for this game
    /// </summary>
    static void DisplayInstructions()
    {
        Console.WriteLine("Your goal is to elminate the opponent's ships");
        Console.WriteLine("You are to write out your inputs as such with spaces: " +
            "\trow column orientation: 1 2 h, 8 9 v, 6 7 v.");
        Console.WriteLine("Rows and columns are numbers between 1 - 10 inclusive.\n'h' = Horizontal & 'v' = Vertical");
        Console.WriteLine("\nAfter setting up, you will enter in your coordinates as such with spaces: " +
            "1 3, 8 3, 4 5");
        Console.WriteLine("\n\nGood luck to both players!\n\n");
    }

    /// <summary>
    /// Alternates between players taking turns
    /// </summary>
    /// <param name="playerID">Takes in which player is playing</param>
    static void TurnProgression(ref int playerID)
    {
        playerID = (playerID % 2) + 1;
    }

    /// <summary>
    /// Used for the ship setup phase
    /// </summary>
    /// <param name="playerID">Taking which player is playing</param>
    static void SetupText(int playerID)
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine($"Player {playerID}, please place your ships. Enter 'x' to exit, 'b' to see the current board layout, or 'i' to see the instructions.");
        Console.WriteLine(new string('-', Console.WindowWidth));
    }

    /// <summary>
    /// Used for the shooting/gameplay phase
    /// </summary>
    /// <param name="playerID">Taking which player is playing</param>
    static void GameplayText(int playerID)
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine($"Player {playerID}. It's your turn. Enter, in order, the row and column you would like to target,'b' to see your ship layout, 'm' to see your shot map, or 'x' to exit, then press <enter>.");
        Console.WriteLine(new string('-', Console.WindowWidth));
    }

    /// <summary>
    /// Makes the boards for both the players and their shot maps
    /// </summary>
    /// <param name="rowDim">Gets the rows of the grid</param>
    /// <param name="colDim">Gets the columns of the grid</param>
    /// <param name="board">Gets the reference of what map is needed to be made</param>
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

    /// <summary>
    /// Determines whether or not the ship is defeated
    /// </summary>
    /// <param name="shipIcon">Gets the character of the ship 'C' for Aircraft Carrier, 'B' for Battleship</param>
    /// <param name="opponentShipMap">Gets the map of the opponent</param>
    /// <param name="currentPlayerShotMap">Gets the map of where the player shot at</param>
    /// <param name="rowDim">Gets the rows of the grid</param>
    /// <param name="colDim">Gets the columns of the grid</param>
    /// <returns>False=If there are no shots on the ship, True=If the ship has shots on all of its length</returns>
    private static bool ShipDefeated(char shipIcon, char[,] opponentShipMap, char[,] currentPlayerShotMap, int rowDim, int colDim)
    {
        for (int row = 0; row < rowDim; row++)
        {
            for (int col = 0; col < colDim; col++)
            {
                if (opponentShipMap[row, col] == shipIcon)
                {
                    if (currentPlayerShotMap[row, col] != 'X')
                    {
                        // If not all location occupied by this Ship have been marked with 'X' on shoot map
                        // The ship has not yet sunk
                        return false;
                    }
                }
            }
        }
        // Only reaching this point after all checks passed
        // Meaning all tiles of this ship are marked with 'X' on shoot map
        // We can now decide this ship sank
        return true;
    }

    /// <summary>
    /// Goes through the array of ship icons and sees which ship is still sunk
    /// </summary>
    /// <param name="hitShipIcon">Gets the opponent's ship map at the exact row and column</param>
    /// <param name="playerShipStatus">Gets the reference of the opponent's ship map</param>
    /// <param name="shipArr">Gets the string array of the ships</param>
    /// <param name="shipIcon">Gets the icon of the ship: 'C' = Aircraft Carrier, 'B' = Battleship</param>
    /// <returns>The ship of the ship array</returns>
    private static string ShipSankUpdate(char hitShipIcon, ref bool[] playerShipStatus, string[] shipArr, char[] shipIcon)
    {
        int index = 0;
        for (int i = 0; i < shipIcon.Length; i++)
        {
            if (hitShipIcon == shipIcon[i])
            {
                // Iterate through the ship icon array, get the index number for the ship that just sank
                // This will be WAY easier with dictionary... but here we go
                index = i;
            }
        }
        playerShipStatus[index] = true;
        return shipArr[index];
    }

    /// <summary>
    /// Checks if there are any ships left on the board for each player
    /// </summary>
    /// <param name="playerShipStatus">Gets the status of the opponent's ships</param>
    /// <returns>False=If there are still ships that exist on the opponent's board 
    /// True=If there are no more ships on the opponent's board</returns>
    private static bool WinCheck(bool[] playerShipStatus)
    {
        foreach (bool ship in playerShipStatus)
        {
            if (!ship)
            {
                // If there are still ship not sunk yet, we cannot decide a winner
                return false;
            }
        }
        // If all of the player's ships return true (all sank), we can declare a winner
        return true;
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

    /// <summary>
    /// Converts the user's inputted coordinates for both rows and columns to be subtracted
    /// by 1 since arrays use the length - 1.
    /// </summary>
    /// <param name="coord">Takes in either the row or column coordinate</param>
    static void InputIndexConversion(ref int coord)
    {
        // This function is used to turn the 1-based user input into 0-based array index
        // Because the user sees 1-10, but the array uses 0-9
        // We need to unify this before passing user input into the array operations
        coord -= 1;
    }


    /// <summary>
    /// Checks to see whether a shot has been hit on the opponent's ships
    /// </summary>
    /// <param name="currentPlayerShotMap">Gets the current player's shot map</param>
    /// <param name="opponentShipMap">Gets the opponent's ship map</param>
    /// <param name="row">Gets the user's row</param>
    /// <param name="col">Gets the user's column</param>
    /// <param name="rowDim">Gets the rows of the map</param>
    /// <param name="colDim">Gets the columns of the map</param>
    /// <returns>False=
    /// 1. A shot is outside the boundaries of the grid
    /// 2. A shot has already been fired at that position
    /// True=If it is inside the boundaries of the grid and has not been fired at the position the player chooses
    /// </returns>
    public static bool ShootFunction(ref char[,] currentPlayerShotMap, ref char[,] opponentShipMap, int row, int col, int rowDim, int colDim)
    {
        // Use 'X' to mark actual hit onto ship
        // Use 'O' to mark missed hits
        // Check out of bound situations
        // Since the coordinates are already converted by InputIndexConversion, it needs to be between 0 and 9
        if (row < 0 || row >= rowDim || col < 0 || col >= colDim)
        {
            Console.WriteLine("Invalid shot.");
            return false;
        }
        else if (currentPlayerShotMap[row, col] == 'X' || currentPlayerShotMap[row, col] == 'O')
        {
            Console.WriteLine("You already fired at this position!");
            return false;
        }
        else
        {
            // Reaching this step, we are actually having valid hits
            if (opponentShipMap[row, col] == '.')
            {
                // The shot missed target
                Console.WriteLine("Miss");
                currentPlayerShotMap[row, col] = 'O';
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("HIT");
                Console.ResetColor();
                Console.WriteLine("(Press <any> key to continue)");
                ConsoleKeyInfo input = Console.ReadKey();
                currentPlayerShotMap[row, col] = 'X';
            }
        }
        return true;
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
    /// <returns>False=
    /// 1. If there is already a ship at that location.
    /// 2. If the placement of the ship is out of bounds, both horizontal and vertical.
    /// True=If the space is empty and is not out of bounds.
    /// </returns>
    public static bool PlaceShipFunction(ref char[,] shipBoard, int row, int col, string direction, int shipSize, char shipIcon, int rowDim, int colDim)
    {
        // Check starting coordinates on board
        if (row < 0 || row > rowDim - 1 || col < 0 || col > colDim - 1)
        {
            Console.WriteLine("Starting coordinates is out of bounds.");
            return false;
        }

        // Check entire ship fits
        if (direction == "h") // Horizontal
        {
            if (col + shipSize > colDim)
            {
                Console.WriteLine("Horizontal ship out of bounds.");
                return false;
            }
        }
        else // Vertical
        {
            if (row + shipSize > rowDim)
            {
                Console.WriteLine("Vertical ship out of bound.");
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
        else // Prevents the player from placing a vertical ship onto of horizontal ships
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
    //     public static void CheckShots(int rowDim, int colDim, ref char[,] currentPlayerShots, ref char[,] currentOpponentShips)
    //     {
    //         // Tells the player whether or not they made direct contact when shooting at a ship
    //         for (int i = 0; i < rowDim; i++)
    //         {
    //             for (int j = 0; j < colDim; j++)
    //             {
    //                 // Gets the player1shots and if they equal the player2's ships locations, output something
    //                 // If there is an X at the current players board and if something is there
    //                 if (currentPlayerShots[i, j] == 'X')
    //                 {
    //                     if (currentOpponentShips[i, j] != '.')
    //                     {
    //                         Console.ForegroundColor = ConsoleColor.Red;
    //                         Console.WriteLine("You hit the enemy ship!");
    //                         ConsoleKeyInfo key = Console.ReadKey();
    //                         Console.ResetColor();
    //                         return;
    //                     }
    //                     else
    //                     {
    //                         break;
    //                     }
    //                 }
    //             }
    //         }
    //     }
}

// When is the first time I can determine win?
// ship or not from shipmap
// shot or not from shootingmap
// arr of int of how many times of the ship has been shot

