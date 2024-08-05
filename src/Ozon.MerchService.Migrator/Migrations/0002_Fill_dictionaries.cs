using FluentMigrator;

namespace Ozon.MerchService.Migrator.Migrations;

[Migration(2)]
public class Fill_dictionaries : ForwardOnlyMigration {
    public override void Up()
    {    
        Execute.Sql(@"
                INSERT INTO request_statuses (id, name)
                VALUES 
                    (1, 'Created'),
                    (2, 'Declined'),
                    (3, 'Queued'),
                    (4, 'Issued')
                ON CONFLICT DO NOTHING
            ");
        
        Execute.Sql(@"
                INSERT INTO request_types (id, name)
                VALUES 
                    (1, 'Manual'),
                    (2, 'Auto')
                ON CONFLICT DO NOTHING
            ");
        
        Execute.Sql(@"
                INSERT INTO clothing_sizes (id, name)
                VALUES 
                    (1, 'XS'),
                    (2, 'S'),
                    (3, 'M'),
                    (4, 'L'),
                    (5, 'XL'),
                    (6, 'XXL')
                ON CONFLICT DO NOTHING
            ");
        
        Execute.Sql(@"
                INSERT INTO merchpacks (id, name, items)
                VALUES 
                    (10, 'WelcomePack', '[{""ItemTypeId"" : 1, ""ItemTypeName"" : ""TShirtStarter"", ""Quantity"" : 1}, {""ItemTypeId"" : 2, ""ItemTypeName"" : ""NotepadStarter"", ""Quantity"" : 1}, {""ItemTypeId"" : 3, ""ItemTypeName"" : ""PenStarter"", ""Quantity"" : 1}, {""ItemTypeId"" : 4, ""ItemTypeName"" : ""SocksStarter"", ""Quantity"" : 1}]'),
                    (20, 'ProbationPeriodEndingPack', '[{""ItemTypeId"" : 5, ""ItemTypeName"" : ""TShirtAfterProbation"", ""Quantity"" : 1}, {""ItemTypeId"" : 6, ""ItemTypeName"" : ""SweatshirtAfterProbation"", ""Quantity"" : 1}]'),
                    (30, 'ConferenceListenerPack', '[{""ItemTypeId"" : 10, ""ItemTypeName"" : ""TShirtСonferenceListener"", ""Quantity"" : 1}, {""ItemTypeId"" : 11, ""ItemTypeName"" : ""NotepadСonferenceListener"", ""Quantity"" : 1}, {""ItemTypeId"" : 12, ""ItemTypeName"" : ""PenСonferenceListener"", ""Quantity"" : 1}]'),
                    (40, 'ConferenceSpeakerPack', '[{""ItemTypeId"" : 7, ""ItemTypeName"" : ""SweatshirtСonferenceSpeaker"", ""Quantity"" : 1}, {""ItemTypeId"" : 8, ""ItemTypeName"" : ""NotepadСonferenceSpeaker"", ""Quantity"" : 1}, {""ItemTypeId"" : 9, ""ItemTypeName"" : ""PenСonferenceSpeaker"", ""Quantity"" : 1}]'),
                    (50, 'VeteranPack', '[{""ItemTypeId"" : 13, ""ItemTypeName"" : ""TShirtVeteran"", ""Quantity"" : 1}, {""ItemTypeId"" : 14, ""ItemTypeName"" : ""SweatshirtVeteran"", ""Quantity"" : 1}, {""ItemTypeId"" : 15, ""ItemTypeName"" : ""NotepadVeteran"", ""Quantity"" : 1}, {""ItemTypeId"" : 16, ""ItemTypeName"" : ""PenVeteran"", ""Quantity"" : 1}, {""ItemTypeId"" : 17, ""ItemTypeName"" : ""CardholderVeteran"", ""Quantity"" : 1}]')
                ON CONFLICT DO NOTHING
            ");
        
        Execute.Sql(@"
                INSERT INTO employees (id, full_name, email)
                VALUES 
                    (1, 'First Last', 'employee@email.com')
                ON CONFLICT DO NOTHING
            ");
        
        Execute.Sql(@"
                INSERT INTO merchpack_requests (id, merchpack_type_id, merchpack_items, employee_id, clothing_size_id, hr_email, request_type_id, requested_at, issued, request_status_id)
                VALUES 
                    (1, 10, '[{""ItemTypeId"" : 1, ""ItemTypeName"" : ""TShirtStarter"", ""Sku"" : 3}, {""ItemTypeId"" : 2, ""ItemTypeName"" : ""NotepadStarter"", ""Sku"" : 41}, {""ItemTypeId"" : 3, ""ItemTypeName"" : ""PenStarter"", ""Sku"" : 42}, {""ItemTypeId"" : 4, ""ItemTypeName"" : ""SocksStarter"", ""Sku"" : 43}]', 1, 3, 'hr@email.com', 2, now() at time zone 'utc', now() at time zone 'utc', 4), 
                    (2, 20, '[{""ItemTypeId"" : 5, ""ItemTypeName"" : ""TShirtAfterProbation"", ""Sku"" : 10}, {""ItemTypeId"" : 6, ""ItemTypeName"" : ""SweatshirtAfterProbation"", ""Sku"" : 16}]', 1, 4, 'hr1@email.com', 2, now() at time zone 'utc', now() at time zone 'utc', 1), 
                    (3, 30, '[{""ItemTypeId"" : 10, ""ItemTypeName"" : ""TShirtСonferenceListener"", ""Sku"" : 27}, {""ItemTypeId"" : 11, ""ItemTypeName"" : ""NotepadСonferenceListener"", ""Sku"" : 46}, {""ItemTypeId"" : 12, ""ItemTypeName"" : ""PenСonferenceListener"", ""Sku"" : 47}]', 1, 3, 'hr2@email.com', 1, now() at time zone 'utc', now() at time zone 'utc', 3), 
                    (4, 40, '[{""ItemTypeId"" : 7, ""ItemTypeName"" : ""SweatshirtСonferenceSpeaker"", ""Sku"" : 21}, {""ItemTypeId"" : 8, ""ItemTypeName"" : ""NotepadСonferenceSpeaker"", ""Sku"" : 44}, {""ItemTypeId"" : 9, ""ItemTypeName"" : ""PenСonferenceSpeaker"", ""Sku"" : 45}]', 1, 3, 'hr@email.com', 1, now() at time zone 'utc', now() at time zone 'utc', 4)
                ON CONFLICT DO NOTHING
            ");
    }
}