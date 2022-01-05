export interface MOFPost {
    populationSize: number;
	crossoverRate: number;
    mutationRate: number;
	numberOfGenerations: number;
}

export interface MOFResponse {
    generation: number;
	fittestIndividualFitness: number;
	generationAvarageFitness: number;
}