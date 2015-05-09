function region(doc)
{
    switch (doc.Location.Region)
    {
        case 0:
            return "North";
        case 1:
            return "Middle";
        case 2:
            return "South";
    }
}