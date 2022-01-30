export interface ConstHandPost { 
	populationSize: number
	crossoverRate: number
	mutationRate: number
	elitismRate: number
	tournamentRate: number
	penaltyConstant: number
	numberOfVariables: number
	inferiorLimit: number
	superiorLimit: number
	numberOfGenerations: number
	challengeFlag: number
}

export interface ConstHandResponse {
    generation: number
	fittestIndividualValue: number
	fittestIndividualConstraint: number
	generationAvarageValue: number
	fittestIndividualVariables: number[]
}