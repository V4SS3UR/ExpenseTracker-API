# ExpenseTracker API

Ce projet est une API développée en **ASP.NET 8 Web API** dans le cadre d'un test technique. Son but est de permettre la gestion des dépenses pour des utilisateurs, avec des fonctionnalités de création et de récupération des dépenses.

## Fonctionnalités

### POST :
- **Créer des dépenses** pour un utilisateur avec une validation approfondie des données d'entrée.

### GET :
- **Lister des dépenses** par utilisateur, avec la possibilité de trier les résultats.

## Base de données

Pour simplifier l'exécution et le partage du projet, j'ai choisi d'utiliser une **InMemoryDatabase**, recréée à chaque démarrage de l'API. Ce choix évite les complications liées à une base de données persistante pour l'intégration et les tests.

### Tables :
- **Users** : Stocke les informations sur les utilisateurs.
- **Expenses** : Stocke les dépenses associées aux utilisateurs.
- **ExpenseTypes** : Définit les types de dépenses possibles.

### Données initiales :
- **2 utilisateurs** :
  - *Anthony Stark* (USD)
  - *Natasha Romanova* (RUB)
- **3 types de dépenses** :
  - *Restaurant*
  - *Hotel*
  - *Misc*

## Exécution du projet

1. Ouvrir la solution dans Visual Studio ou autre IDE.
2. Démarrer le projet **ExpenseTracker**, ce qui lancera l'API accompagnée d'une interface **Swagger**.
3. Accéder à l'interface Swagger à l'URL suivante : [https://localhost:7095/swagger/index.html](https://localhost:7095/swagger/index.html).
4. Accepter le certificat IIS dans le navigateur si nécessaire.

## Utilisation de l'API

### Ajouter une dépense

Pour ajouter une dépense, il faut faire une requête **POST** à l'URL suivante :  
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

#### Validations effectuées :
- **Date** : La date ne peut pas être dans le futur.
- **Date** : La date ne peut pas être antérieure à 3 mois.
- **Commentaire** : Obligatoire.
- **Currency** : Obligatoire et doit respecter le format ISO 4217.
- **Doublon** : Pas de doublon pour une dépense de même date et montant pour un même utilisateur.
- **ExpenseType** : Doit exister dans la base de données.
- **User** : Doit exister dans la base de données.
- **Currency** : Doit correspondre à la devise de l'utilisateur.

En cas de validation échouée, une `AggregateException` est levée avec une liste d'erreurs détaillées.

### Lister les dépenses

Pour lister les dépenses d'un utilisateur, faire une requête **GET** sur l'URL :  
`https://localhost:7095/api/Expenses`

#### Paramètres :
- `userId` (int) : Identifiant de l'utilisateur.
  - 1 pour *Anthony Stark*.
  - 2 pour *Natasha Romanova*.
- `sort` (string) : Champ de tri.
  - `"date"` : Trie les dépenses par date.
  - `"amount"` : Trie les dépenses par montant.

### Créer et obtenir des utilisateurs

En raison de problèmes initiaux de connexion à une base de données locale, j'ai ajouté un controller permettant de **créer** et **obtenir** des utilisateurs, ce qui m'a aidé à déboguer l'application.

### Autres services

#### ExpenseTypeService
- Permet de **créer** et **obtenir** des types de dépenses.
- **Validations** :
  - Le nom est obligatoire et unique.

#### UserService
- Permet de **créer** et **obtenir** des utilisateurs.
- **Validations** :
  - `firstName` est obligatoire.
  - `lastName` est obligatoire.
  - `currency` est obligatoire et doit respecter le format ISO 4217.

## Tests unitaires

Les tests unitaires sont situés dans le projet **ExpenseTrackerTests** et suivent le pattern **Arrange-Act-Assert**.

### Stratégie de test
Initialement, j'ai utilisé des mocks pour simuler les opérations des repositories, mais j'ai opté pour une base **InMemoryDatabase** pour effectuer des tests plus représentatifs du comportement réel de l'application.

Chaque test utilise une base distincte pour garantir l'isolement et éviter les interférences entre les tests.

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

### Packages utilisés :
- **EntityFramework (8.0.8)** : Utilisé pour la gestion des entités et des opérations sur la base de données. L'InMemoryDatabase permet des tests rapides sans configuration de base de données.
- **Moq** : Utilisé initialement pour des tests unitaires mais finalement abandonné au profit des tests réels avec une InMemoryDatabase.
- **ISO.4217.CurrencyCodes** : Utilisé pour valider les formats de devise selon la norme ISO 4217.

### DTOs :
Pour éviter d'exposer des informations inutiles ou sensibles, j'ai mis en place des **DTOs** pour les opérations **GET** et **POST** :
- **ExpenseCreateDTO** : Utilisé lors de la création d'une dépense.
- **ExpenseGetDTO** : Utilisé lors de la récupération des dépenses, incluant le nom complet de l'utilisateur (`FirstName LastName`).

## Améliorations potentielles

- **OrderBy des dépenses** : Ajouter la possibilité de trier les dépenses par ordre croissant ou décroissant.
- **Erreur custom** : Créer une exception personnalisée pour gérer et retourner les erreurs de validation de manière plus claire et structurée.