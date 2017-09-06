using System;
using System.Collections.Generic;

[Serializable]
public class Comment : IEquatable<Comment>
{
    public int ID;
    public string Timestamp;
    public int User;
    public string Demographics;
    public string Location;
    public float X;
    public float Y;
    public float Z;
    public string Petal;
    public string Text;

    public Comment(string timestamp, int user, string demographics, string location, float x, float y, float z, string petal, string text)
    {
        ID = -1;
        Timestamp = timestamp;
        User = user;
        Demographics = demographics;
        Location = location;
        X = x;
        Y = y;
        Z = z;
        Petal = petal;
        Text = text;
    }

    public bool Equals(Comment other)
    {
        if (other == null)
            return false;
        return (ID == other.ID);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        
        return Equals(obj as Comment);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator == (Comment c1, Comment c2)
    {
        if (((object) c1) == null || ((object) c2) == null)
            return Equals(c1, c2);

        return c1.Equals(c2);
    }

    public static bool operator != (Comment c1, Comment c2)
    {
        if (((object) c1) == null || ((object) c2) == null)
            return !Equals(c1, c2);

        return !(c1.Equals(c2));
    }
}