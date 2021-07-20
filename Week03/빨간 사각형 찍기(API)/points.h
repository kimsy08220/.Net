class pt
{
private:
	int x;
	int y;
public:
	void Set(int x, int y)
	{
		this->x = x;	// ambiguity(모호성)을 제거하기 위해 this를 사용
		this->y = y;	// y = y;하면 compile error는 없으나 문제 발생
	}
	
	POINT Get()
	{
		POINT p;
		p.x = x;
		p.y = y;
		return p;
	}
};

class mypoints : public pt 
{
private:
	pt p[1000];			// pt 클래스의 (instance, object) 배열
	int iCount;
public:
	void Add(int x, int y)
	{
		if (iCount < 1000)
		{
			p[iCount].Set(x, y);
			iCount++;
		}
	}
	void Draw(HDC hdc)
	{
		HBRUSH hBrush;
		hBrush = CreateSolidBrush(RGB(255, 0, 0));
		SelectObject(hdc, hBrush);
		for (int i = 0; i < iCount; i++)
			Rectangle(hdc, p[i].Get().x - 10, p[i].Get().y - 10, p[i].Get().x + 10, p[i].Get().y + 10);
	}
};