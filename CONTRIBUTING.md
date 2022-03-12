# Contribution guide

## feature structure

- All features should be contained in the `WBM/features` folder.

### Feature functions

- All features can have at most one function to be called in each event functions.
- feature function should have the following format: `<prefix><feature name>`

  | Event function | feature function prefix |
  | -------------: | :---------------------- |
  |        `Awake` | `init`                  |
  |        `Start` | `setup`                 |
  |       `Update` | `do`                    |
  |        `OnGUI` | `draw`                  |
  |    `onDestroy` | `destroy`               |
