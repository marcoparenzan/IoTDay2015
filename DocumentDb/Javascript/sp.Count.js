function count(filterQuery, continuationToken) {
    var collection = getContext().getCollection();
    var maxResult = 25; // MAX number of docs to process in one batch, when reached, return to client/request continuation. 
    // intentionally set low to demonstrate the concept. This can be much higher. Try experimenting.
    // We've had it in to the high thousands before seeing the stored proceudre timing out.

    // The number of documents counted.
    var result = 0;

    tryQuery(continuationToken);

    // Helper method to check for max result and call query.
    function tryQuery(nextContinuationToken) {
        var responseOptions = { continuation: nextContinuationToken, pageSize: maxResult };

        // In case the server is running this script for long time/near timeout, it would return false,
        // in this case we set the response to current continuation token, 
        // and the client will run this script again starting from this continuation.
        // When the client calls this script 1st time, is passes empty continuation token.
        if (result > maxResult || !query(responseOptions)) {
            setBody(nextContinuationToken);
        }
    }

    function query(responseOptions) {
        // For empty query string, use readDocuments rather than queryDocuments -- it's faster as doesn't need to process the query.
        return (filterQuery && filterQuery.length) ?
            collection.queryDocuments(collection.getSelfLink(), filterQuery, responseOptions, onReadDocuments) :
            collection.readDocuments(collection.getSelfLink(), responseOptions, onReadDocuments);
    }

    // This is callback is called from collection.queryDocuments/readDocuments.
    function onReadDocuments(err, docFeed, responseOptions) {
        if (err) {
            throw 'Error while reading document: ' + err;
        }

        // Increament the number of documents counted so far.
        result += docFeed.length;

        // If there is continuation, call query again with it, 
        // otherwise we are done, in which case set continuation to null.
        if (responseOptions.continuation) {
            tryQuery(responseOptions.continuation);
        } else {
            setBody(null);
        }
    }

    // Set response body: use an object the client is expecting (2 properties: result and continuationToken).
    function setBody(continuationToken) {
        var body = { count: result, continuationToken: continuationToken };
        getContext().getResponse().setBody(body);
    }
}
