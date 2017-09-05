restore database arda_kanban from disk='c:\ardakanban.bak'
with 
move 'arda_kanban_Data' to 'C:\DB\arda_kanban.mdf',
move 'arda_kanban_Log' to 'C:\DB\arda_kanban.ldf'

restore database arda_permissions from disk='c:\ardapermissions.bak'
with 
move 'arda_permissions_Data' to 'C:\DB\arda_permissions.mdf',
move 'arda_permissions_Log' to 'C:\DB\arda_permissions.ldf'
