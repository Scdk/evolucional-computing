export interface ContParamsPost { 
	populationSize: number
	crossoverRate: number
	mutationRate: number
	elitismRate: number
	tournamentRate: number
	numberOfVariables: number
	inferiorLimit: number
	superiorLimit: number
	numberOfGenerations: number
}

export interface ContParamsResponse {
    generation: number
	fittestIndividualFitness: number
	generationAvarageFitness: number
	fittestIndividualVariables: number[]
}