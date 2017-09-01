using System.Collections;
using System.Collections.Generic;
using System;

namespace opencreature {
public class Creaturedb {
	const string PREFIX = "CREATURE.DB";
	private static bool init = false;
	private static SqliteWrapper db;
    public static Dictionary<string, TypeCastDictionary<int, DeserializedElement>> TABLE_OBJECTS;
	
	private Creaturedb(){}
	public static bool initialize() {
	    if (init) return false;
	    var log = log4net.LogManager.GetLogger("Creature.db");
		log.Debug(Globals.binary_location);
	    db = new SqliteWrapper(
			"Data Source="+Globals.binary_directory+"creature.db;"+
		    "Version=3;"+
			"Read Only=True;"+
			"FailIfMissing=True;"
	    );
	    log.Info("Deserializing creature.db");
	    long count;

	    
	    #region Init
	    count = Element.init(
	        db["SELECT * FROM types"],
	        db["SELECT * FROM type_bonus"]
	    );
	    log.Debug(count + " Elements loaded");

	    count = Species.init(
	        db["SELECT * FROM species;"],
	        db["SELECT * FROM level_moves"],
	        db["SELECT * FROM evolution"]
	    );
	    log.Debug(count + " Species loaded");

	    count = Move.init(db["SELECT * FROM moves"]);
	    log.Debug(count + " Moves loaded");

	    count = Ability.init(db["SELECT * FROM abilities"]);
	    log.Debug(count + " Abilities loaded");

	    count = Effect.init(db["SELECT * FROM effects"]);
	    log.Debug(count + " Effects loaded");

	    count = Nature.init(db["SELECT * FROM natures"]);
	    log.Debug(count + " Natures loaded");

	    count = EggGroup.init(db["SELECT * FROM egg_groups"]);
	    log.Debug(count + " Egg Types loaded");

	    count = ItemType.init(db["SELECT * FROM item_type"]);
	    log.Debug(count + " Item Types loaded");

	    count = Item.init(db["SELECT * FROM items"]);
	    log.Debug(count + " Items loaded");
	    
	    count = Creature.init_unique(db["SELECT * FROM unique_creature"]);
	    log.Debug(count + " Creatures loaded");

	    count = PlotFlag.init(db["SELECT * FROM plot_flag"]);
	    log.Debug(count + " Plot Flags loaded");

	    count = Npc.init(db["SELECT * FROM npc"]);
	    log.Debug(count + " NPCs loaded");

	    #endregion

	    #region Link

	    log.Info("Linking objects");

	    Species.link();
	    log.Debug("Species linked");

	    Ability.link();
	    log.Debug("Abilities linked");

	    Move.link();
	    log.Debug("Moves linked");

	    Npc.link();
	    log.Debug("Npcs linked");

	    Item.link();
	    log.Debug("Items linked");

	    #endregion

        TABLE_OBJECTS = new Dictionary<string, TypeCastDictionary<int, DeserializedElement>>() {
                {"Elements"     , Element.TYPES         },
                {"Abilities"    , Ability.ABILITIES     },
                {"Egg Groups"   , EggGroup.EGG_GROUPS   },
                {"Items"        , Item.ITEMS            },
                {"Item Classes" , ItemType.ITEM_TYPES   },
                {"Moves"        , Move.MOVES            },
                {"Natures"      , Nature.NATURES        },
                {"Effects"      , Effect.EFFECTS        },
                {"Species"      , Species.SPECIES       },
                {"NPCs"         , Npc.NPCS              },
                {"Plot Flags"   , PlotFlag.PLOT_FLAG_ID },
        };

	    init = true;
	    log.Info("Creature.db fully deserialized!");
	    return init;
	}
}
}