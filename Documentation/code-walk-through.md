## Minesweeper Presentation

* Program : entry point
* Engine: creates the main game components and runs the game loop
* UIManager: responsible for taking user I/O
    * **Bridge** with IRenderer and IUserInputReader
* ConsoleRenderer: **Adapter** for System.Console
* CommandParser: creates the proper Command, has virtual method
* ICommand: commands invoke public methods of the receiver Game
* CommandExecutor: part of the **Command** pattern
* MinesweeperGame:
    * **Facade** for the UIManager, Minefield and ScoreBoard classes
	* public methods are called by the commands
	* uses **Factory Method** (CreateMinefield) to create the minefield.
* MinesweeperGameEasy: Overrides the factory method to return MinefieldEasy
* MinefieldEasy: overrides OpenCell by calling base and then OpenEmptyCellsRecursive
* Minefield: holds a collection of cells (IList<ICell>)
    * methods OpenCell, FlagCell and GetImage (CellImage enum)
* static class Messages: messages as const
* ScoreBoard: AddScore and TopScores (deep copy)
* Minesweeper.Lib: class library from classes that don't depend on other classes
* Cell: IsOpened, IsMined and IsFlagged; Disarm method for swapping
* RandomGeneratorProvider: **Singleton** because we only need one random generator 
* Array.Shuffle:  implements **Strategy** pattern - takes IRandomGeneratorProvider
* DRY:
    * minefield is drawn by the same method regardless of the game state
	* acting on a cell (open or flag) works with a delegate to avoid repetition
	* the Shuffle method is implemented as extension method
* Unit Tests: 90% code coverage.
* Game demo: show flag, boom and recursive opening
