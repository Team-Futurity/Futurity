public static class TextUtility
{
	public static bool IsWhitespace(this char character)
	{
		switch (character)
		{
			case '\u0020':
			case '\u00A0':
			case '\u1680':
			case '\u2000':
			case '\u2001':
			case '\u2002':
			case '\u2003':
			case '\u2004':
			case '\u2005':
			case '\u2006':
			case '\u2007':
			case '\u2008':
			case '\u2009':
			case '\u200A':
			case '\u202F':
			case '\u205F':
			case '\u3000':
			case '\u2028':
			case '\u2029':
			case '\u0009':
			case '\u000A':
			case '\u000B':
			case '\u000C':
			case '\u000D':
			case '\u0085':
				{
					return true;
				}

			default:
				{
					return false;
				}
		}
	}

	public static string RemoveWhitespaces(this string text)
	{
		int textLength = text.Length;

		char[] textCharacters = text.ToCharArray();

		int currentWhitespacelessTextLength = 0;

		for (int currentCharacterIndex = 0; currentCharacterIndex < textLength; ++currentCharacterIndex)
		{
			char currentTextCharacter = textCharacters[currentCharacterIndex];

			// ���� ���ڰ� ����(White Space)��� ��ŵ
			if (currentTextCharacter.IsWhitespace())
			{
				continue;
			}

			textCharacters[currentWhitespacelessTextLength++] = currentTextCharacter;
		}

		// ������ ������ ���ο� ���ڿ� ��ȯ
		return new string(textCharacters, 0, currentWhitespacelessTextLength);
	}

	public static string RemoveSpecialCharacters(this string text)
	{
		int textLength = text.Length;

		char[] textCharacters = text.ToCharArray();

		int currentWhitespacelessTextLength = 0;

		for (int currentCharacterIndex = 0; currentCharacterIndex < textLength; ++currentCharacterIndex)
		{
			char currentTextCharacter = textCharacters[currentCharacterIndex];

			// C# ������ ���� ����(Letter), ����(Digit)���� �ƴ����� ����(White Space) ���θ� �Ǻ���.
			// ������ �ƴ� Ư�� ������ ��� ��ŵ
			if (!char.IsLetterOrDigit(currentTextCharacter) && !currentTextCharacter.IsWhitespace())
			{
				continue;
			}

			textCharacters[currentWhitespacelessTextLength++] = currentTextCharacter;
		}

		// Ư�����ڸ� ������ ���ο� ���ڿ� ��ȯ
		return new string(textCharacters, 0, currentWhitespacelessTextLength);
	}
}