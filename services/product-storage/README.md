Listing Object:

    {
    Listing
        {
            listing_guid: UUID, 	///// Check UUID type is valid for guid from frontend
	    person_guid: UUID,          ///// Check UUID type is valid for guid from frontend
	    title: String,
            description: String, 
            x_length_in_mm: Integer,
            y_width_in_mm: Integer,
            z_height_in_mm: Integer,
	    wear_rating
                {
		    rating: Integer,
                    condition: String
                }
            price_dkk: double,
	    city: String,
	    modified_date: TimeStamp,
	    colors
                [
		    name: String,
                    href: String
		],
            subcategories
                [
                    name: String,
                    category: String
                ],
	    images
                [
	            url: String, 
                ]           
        }
    }