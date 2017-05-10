iwr http://localhost:2768/swagger/v2/swagger.json -o kanban.json

autorest -Input kanban.json -CodeGenerator CSharp -OutputDirectory Kanban -Namespace ArdaSDK.Kanban -ClientName KanbanClient