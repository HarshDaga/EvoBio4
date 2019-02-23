# Evo Bio 4

## Variables used

- `CooperatorQuantity` Starting quantity of cooperator type

- `DefectorQuantity` Starting quantity of defector type

- `SdQuality` Standard deviation of distribution from which an individual’s quality is
  generated

- `Y` Fraction of fitness lost that is used by individuals in the population

- `Relatedness` The assortment (“relatedness”) in the population

- `PercentileCutoff` Quality percentile cutoff for reproduction

- `Z` Used in computing confidence intervals

- `MaxTimeSteps` Maximum number of time steps to simulate in a run

- `Runs` Number of runs to simulate

- `IncludeConfidenceIntervals` Should confidence intervals be computed? `true`/`false`


## Strategy Collection

  In each time step, several tasks are performed based on one of several strategies.
Here's a list of all the tasks and their corresponding strategies:

- `Survival`- Select one individual from the population to perish
  - `EquiProbable` - All individuals save the same probability of surviving
  - `QualityProportional` - All but one individuals are chosen to survive with their probability being proportional to their quality
  - `FitnessProportional` - All but one individuals are chosen to survive with their probability being proportional to their fitness
  - `QualityInverselyProportional` - One individual is chosen to perish with the probability being *Inversely* proportional to its quality
  - `FitnessInverselyProportional` - One individual is chosen to perish with the probability being *Inversely* proportional to its fitness
- `Fitness` - Defines how the `Fitness` value of an individual is computed
  - `Default` - The default fitness formula as written in the manuscript
  - `NonReproducingHave0Fitness` - Non reproducing cooperators are allotted 0 fitness
- `Reproduction` - Select one individual from the population to reproduce
  - `QualityProportional` - The probability of an individual being chosen to reproduce is directly proportional to its quality
- `PostProcess` - Some tasks to be more performed at the end of each time step
  - `DoNothing` - Nothing is done
  - `Shuffle` - The list of individuals is shuffled

