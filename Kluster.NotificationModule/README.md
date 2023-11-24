# Notification Module

This module has one job - send notifications. The only service shared with other modules is `INotificationService`. It is called for any set of notifications.

## How To - Send E-Mails
1. Create a messaging contract in the Shared library.
2. Create a consumer for the contract created in 1.
3. In `NotificationService` (and its interface), create a method to handle that class of email.
4. Create the related Template in Kluster.Host/Templates. Ensure the template has values to be replaced, enclose such values in {braces}.
5. Use `mailService.LoadTemplate(pathToTemplate)` to load the template.
6. Use `string.Replace()` to replace the placeholders with the actual values.
7. Return `mailService.SendAsync()` with a fitting subject. The sender details are set by default, so don't worry about it.