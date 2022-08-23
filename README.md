# Introduction 
This is a console application to calculate and print a bill

# Instructions
Write your inputs with the next format ```[Quantity] [Description] at [Price]``` where quantity should be a integer number and price a decimal, example '1 Book at 12.49'.
To end and get your bill write the word 'END'
To finish your session write the word 'EXIT'

### Assumptions
1. No persistency was required, hence every run is a new and only bill.
2. Item categorization is made by a name match with a simple list of words for tax exception items. This list could be a persistent object like a data table but in a real scenario item categorization is not done on the fly.
3. Program class works as a Front end, so the is printing a service output.
4. Service has a list of items for each session mocking the repository functionality.