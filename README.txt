Foi optado por fazer o código em uma API dotnet 6.0, portanto se faz necessária a instalação do dotnet.

Após instalado deve-se rodar o comando "dotnet run".

Depois que a API for iniciada é enviado um GET com um body Json de configurações como esse abaixo para o endereço "https://localhost:5001/simple-exemple"

{
	"populationSize": 50,
	"crossoverRate": 0.6,
    "mutationRate": 0.1,
	"numberOfGenerations": 5
}
