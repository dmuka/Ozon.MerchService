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
    }
}