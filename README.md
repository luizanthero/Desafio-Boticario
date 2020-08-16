# Desafio-Boticario
Web API em .Net Core 3.1 utilizando Swagger para documentação

### Tabelas Importantes
* __ParametroSistema__ 
> Tabela que armazena todos os parâmetros utilizados no código do sistema, Exemplo: CPF de Revendedor Coringa: 153.509.460-56

* __TipoHistorico__
> Tabela que armazena todos os tipo de Histório possiveis, Exemplo: Criação, Exclusão, Alteração

* __StatusCompra__
> Tabela que armazena todos os status de Compra possiveis, Exemplo: Em validação, Aprovado

* __RegraCashback__
> Tabela que armazena todas as regras de Cashback disponiveis, Exemplo: Para até 1.000 reais em compras, o revendedor(a) receberá 10% de cashback do valor vendido no período de um mês;

__Aconselho para as tabelas acima, depois de executar o Migrations para criação do banco, seja executado o arquivo "InsertInicial.sql" no diretório "./boticario.DAL/ScriptSQL"__
