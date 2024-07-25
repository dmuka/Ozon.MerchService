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
                    (1, 'WelcomePack', ARRAY[1, 2, 3, 4]),
                    (2, 'ProbationPeriodEndingPack', ARRAY[5, 6]),
                    (3, 'ConferenceListenerPack', ARRAY[10, 11, 12]),
                    (4, 'ConferenceSpeakerPack', ARRAY[7, 8, 9]),
                    (5, 'VeteranPack', ARRAY[13, 14, 15, 16, 17])
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
                    (1, 10, '[{""ItemType"" : 1, ""Sku"" : 3}, {""ItemType"" : 2, ""Sku"" : 41}, {""ItemType"" : 3, ""Sku"" : 42}, {""ItemType"" : 4, ""Sku"" : 43}]', 1, 3, 'hr@email.com', 2, now() at time zone 'utc', now() at time zone 'utc', 4), 
                    (2, 20, '[{""ItemType"" : 5, ""Sku"" : 10}, {""ItemType"" : 6, ""Sku"" : 16}]', 1, 4, 'hr1@email.com', 2, now() at time zone 'utc', now() at time zone 'utc', 1), 
                    (3, 30, '[{""ItemType"" : 10, ""Sku"" : 27}, {""ItemType"" : 11, ""Sku"" : 46}, {""ItemType"" : 12, ""Sku"" : 47}]', 1, 3, 'hr2@email.com', 1, now() at time zone 'utc', now() at time zone 'utc', 3), 
                    (4, 40, '[{""ItemType"" : 7, ""Sku"" : 21}, {""ItemType"" : 8, ""Sku"" : 44}, {""ItemType"" : 9, ""Sku"" : 45}]', 1, 3, 'hr@email.com', 1, now() at time zone 'utc', now() at time zone 'utc', 4)
                ON CONFLICT DO NOTHING
            ");
        
        Execute.Sql(@"
                INSERT INTO merchpacks_items (id, item_type_id, item_type_name)
                VALUES 
                    (1, 1, 'TShirtStarter'),
                    (2, 2, 'NotepadStarter'),
                    (3, 3, 'PenStarter'),
                    (4, 4, 'SocksStarter'), 
                    (5, 5, 'TShirtAfterProbation'), 
                    (6, 6, 'SweatshirtAfterProbation'), 
                    (7, 10, 'TShirtСonferenceListener'), 
                    (8, 11, 'NotepadСonferenceListener'), 
                    (9, 12, 'PenСonferenceListener'), 
                    (10, 7, 'SweatshirtСonferenceSpeaker'), 
                    (11, 8, 'NotepadСonferenceSpeaker'), 
                    (12, 9, 'PenСonferenceSpeaker'),
                    (13, 13, 'TShirtVeteran'),
                    (14, 14, 'SweatshirtVeteran'),
                    (15, 15, 'NotepadVeteran'),
                    (16, 16, 'PenVeteran'),
                    (17, 17, 'CardHolderVeteran') 
                    
                ON CONFLICT DO NOTHING
            ");
    }
}