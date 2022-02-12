export interface DifEvoPost { 
	populationSize: number
	crossoverRate: number
	numberOfVariables: number
	inferiorLimit: number
	superiorLimit: number
	fValue: number
	numberOfGenerations: number
}

export interface DifEvoResponse {
    generation: number
	fittestIndividualFitness: number
	generationAvarageFitness: number
	fittestIndividualVariables: number[]
}