using System;

public interface IShowable
{
    void Show(Action callBack = null);
    void Hide(Action callBack = null);
}
