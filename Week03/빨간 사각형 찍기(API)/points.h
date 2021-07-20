class pt
{
private:
	int x;
	int y;
public:
	void Set(int x, int y)
	{
		this->x = x;	// ambiguity(��ȣ��)�� �����ϱ� ���� this�� ���
		this->y = y;	// y = y;�ϸ� compile error�� ������ ���� �߻�
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
	pt p[1000];			// pt Ŭ������ (instance, object) �迭
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