function normalize() {
    var collection = getContext().getCollection();
    var collectionLink = collection.getSelfLink();
    var doc = getContext().getRequest().getBody();

    var newDoc = {
    	"Sensor": {
    		"Id": doc.sensorId,
    		"Class": 0
    	},
    	"Degree": {
    		"Value": doc.degreeValue,
    		"Type": 0
    	},
    	"Location": {
    		"Name": doc.locationName,
    		"Region": doc.locationRegion,
    		"Longitude": doc.locationLong,
    		"Latitude": doc.locationLat
    	},
		"id": doc.id
    };

    // Update the request -- this is what is going to be inserted.
    getContext().getRequest().setBody(newDoc);
}
