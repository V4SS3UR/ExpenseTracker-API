# ExpenseTracker API

Ce projet est une API d�velopp�e en **ASP.NET 8 Web API** dans le cadre d'un test technique. Son but est de permettre la gestion des d�penses pour des utilisateurs, avec des fonctionnalit�s de cr�ation et de r�cup�ration des d�penses.

## Fonctionnalit�s

### POST :
- **Cr�er des d�penses** pour un utilisateur avec une validation approfondie des donn�es d'entr�e.

### GET :
- **Lister des d�penses** par utilisateur, avec la possibilit� de trier les r�sultats.

## Base de donn�es

Pour simplifier l'ex�cution et le partage du projet, j'ai choisi d'utiliser une **InMemoryDatabase**, recr��e � chaque d�marrage de l'API. Ce choix �vite les complications li�es � une base de donn�es persistante pour l'int�gration et les tests.

### Tables :
- **Users** : Stocke les informations sur les utilisateurs.
- **Expenses** : Stocke les d�penses associ�es aux utilisateurs.
- **ExpenseTypes** : D�finit les types de d�penses possibles.

### Donn�es initiales :
- **2 utilisateurs** :
  - *Anthony Stark* (USD)
  - *Natasha Romanova* (RUB)
- **3 types de d�penses** :
  - *Restaurant*
  - *Hotel*
  - *Misc*

## Ex�cution du projet

1. Ouvrir la solution dans Visual Studio ou autre IDE.
2. D�marrer le projet **ExpenseTracker**, ce qui lancera l'API accompagn�e d'une interface **Swagger**.
3. Acc�der � l'interface Swagger � l'URL suivante : [https://localhost:7095/swagger/index.html](https://localhost:7095/swagger/index.html).
4. Accepter le certificat IIS dans le navigateur si n�cessaire.

## Utilisation de l'API

### Ajouter une d�pense

Pour ajouter une d�pense, il faut faire une requ�te **POST** � l'URL suivante :  
`https://localhost:7095/api/Expenses`

Exemple de payload JSON :

```json
{
  "date": "2024-09-19T00:00:00.000Z",
  "amount": 10,
  "currency": "USD",
  "comment": "test",
  "userId": 1,
  "expenseTypeId": 1
}
```

#### Validations effectu�es :
- **Date** : La date ne peut pas �tre dans le futur.
- **Date** : La date ne peut pas �tre ant�rieure � 3 mois.
- **Commentaire** : Obligatoire.
- **Currency** : Obligatoire et doit respecter le format ISO 4217.
- **Doublon** : Pas de doublon pour une d�pense de m�me date et montant pour un m�me utilisateur.
- **ExpenseType** : Doit exister dans la base de donn�es.
- **User** : Doit exister dans la base de donn�es.
- **Currency** : Doit correspondre � la devise de l'utilisateur.

En cas de validation �chou�e, une `AggregateException` est lev�e avec une liste d'erreurs d�taill�es.

### Lister les d�penses

Pour lister les d�penses d'un utilisateur, faire une requ�te **GET** sur l'URL :  
`https://localhost:7095/api/Expenses`

#### Param�tres :
- `userId` (int) : Identifiant de l'utilisateur.
  - 1 pour *Anthony Stark*.
  - 2 pour *Natasha Romanova*.
- `sort` (string) : Champ de tri.
  - `"date"` : Trie les d�penses par date.
  - `"amount"` : Trie les d�penses par montant.

### Cr�er et obtenir des utilisateurs

En raison de probl�mes initiaux de connexion � une base de donn�es locale, j'ai ajout� un controller permettant de **cr�er** et **obtenir** des utilisateurs, ce qui m'a aid� � d�boguer l'application.

### Autres services

#### ExpenseTypeService
- Permet de **cr�er** et **obtenir** des types de d�penses.
- **Validations** :
  - Le nom est obligatoire et unique.

#### UserService
- Permet de **cr�er** et **obtenir** des utilisateurs.
- **Validations** :
  - `firstName` est obligatoire.
  - `lastName` est obligatoire.
  - `currency` est obligatoire et doit respecter le format ISO 4217.

## Tests unitaires

Les tests unitaires sont situ�s dans le projet **ExpenseTrackerTests** et suivent le pattern **Arrange-Act-Assert**.

### Strat�gie de test
Initialement, j'ai utilis� des mocks pour simuler les op�rations des repositories, mais j'ai opt� pour une base **InMemoryDatabase** pour effectuer des tests plus repr�sentatifs du comportement r�el de l'application.

Chaque test utilise une base distincte pour garantir l'isolement et �viter les interf�rences entre les tests.

### Liste des tests :

#### CreateExpense :
- ShouldSucceed_IfValid		
- ShouldThrowException_IfCommentIsMissing
- ShouldThrowException_IfCurrencyDoesNotMatchUserCurrency
- ShouldThrowException_IfCurrencyIsInvalid
- ShouldThrowException_IfCurrencyIsMissing
- ShouldThrowException_IfDateIsInFuture	
- ShouldThrowException_IfDateIsOlderThanThreeMonths
- ShouldThrowException_IfExpenseIsDuplicate
- ShouldThrowException_IfExpenseTypeDoesNotExist
- ShouldThrowException_IfUserDoesNotExist

#### GetExpensesByUserAsync :
- ShouldReturnEmptyList_WhenNoExpenses		
- ShouldReturnExpenses_WhenValidUserId	
- ShouldThrowException_WhenInvalidSortField

#### CreateUserAsync :
- ShouldReturnUser_WhenValidUserIsProvided		
- ShouldThrowException_WhenCurrencyIsMissing		
- ShouldThrowException_WhenFirstNameIsMissing	
- ShouldThrowException_WhenInvalidCurrency		
- ShouldThrowException_WhenLastNameIsMissing

#### GetAllUsersAsync :
- ShouldReturnSortedUsers
- ShouldThrowException_WhenInvalidSortField
	
#### CreateExpenseTypeAsync :
- ShouldCreateExpenseType_WhenValid	
- ShouldThrowException_WhenExpenseTypeExists	
- ShouldThrowException_WhenNameIsEmpty	

#### GetAllExpensesTypeAsync :
- ShouldReturnAllExpenseTypes		

## Choix techniques

### Packages utilis�s :
- **EntityFramework (8.0.8)** : Utilis� pour la gestion des entit�s et des op�rations sur la base de donn�es. L'InMemoryDatabase permet des tests rapides sans configuration de base de donn�es.
- **Moq** : Utilis� initialement pour des tests unitaires mais finalement abandonn� au profit des tests r�els avec une InMemoryDatabase.
- **ISO.4217.CurrencyCodes** : Utilis� pour valider les formats de devise selon la norme ISO 4217.

### DTOs :
Pour �viter d'exposer des informations inutiles ou sensibles, j'ai mis en place des **DTOs** pour les op�rations **GET** et **POST** :
- **ExpenseCreateDTO** : Utilis� lors de la cr�ation d'une d�pense.
- **ExpenseGetDTO** : Utilis� lors de la r�cup�ration des d�penses, incluant le nom complet de l'utilisateur (`FirstName LastName`).

## Am�liorations potentielles

- **OrderBy des d�penses** : Ajouter la possibilit� de trier les d�penses par ordre croissant ou d�croissant.
- **Erreur custom** : Cr�er une exception personnalis�e pour g�rer et retourner les erreurs de validation de mani�re plus claire et structur�e.