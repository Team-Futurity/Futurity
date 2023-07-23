using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PositioningTestWindow : EditorWindow
{
	public int test;

	[MenuItem("Window/UI Toolkit/Positioning Test Window")]
	public static void ShowExample()
	{
		var wnd = GetWindow<PositioningTestWindow>();
		wnd.titleContent = new GUIContent("Positioning Test Window");
	}

	private void t()
	{
		RuntimePanelUtils.ScreenToPanel(rootVisualElement.panel, Pointer.current.position.ReadValue());
	}

	public void CreateGUI()
	{
		/*for (int i = 0; i < 2; i++)
		{
			var temp = new VisualElement();
			temp.style.width = 70;
			temp.style.height = 70;
			temp.style.marginBottom = 2;
			temp.style.backgroundColor = Color.gray;
			rootVisualElement.Add(temp);
		}

		// Relative positioning
		var relative = new Label("Relative\nPos\n25, 0");
		relative.style.width = 70;
		relative.style.height = 70;
		relative.style.left = 25;
		relative.style.marginBottom = 2;
		relative.style.backgroundColor = new Color(0.2165094f, 0, 0.254717f);
		rootVisualElement.Add(relative);

		for (int i = 0; i < 2; i++)
		{
			var temp = new VisualElement();
			temp.style.width = 70;
			temp.style.height = 70;
			temp.style.marginBottom = 2;
			temp.style.backgroundColor = Color.gray;
			rootVisualElement.Add(temp);
		}

		// Absolute positioning
		var absolutePositionElement = new Label("Absolute\nPos\n25, 25");
		absolutePositionElement.style.position = Position.Absolute;
		absolutePositionElement.style.top = 25;
		absolutePositionElement.style.left = 25;
		absolutePositionElement.style.width = 70;
		absolutePositionElement.style.height = 70;
		absolutePositionElement.style.backgroundColor = Color.black;
		rootVisualElement.Add(absolutePositionElement);


		//VisualElement picked = rootVisualElement.panel.Pick(pos);
		//VisualElement picked = doc.rootVisualElement.panel.Pick(Pointer.current.position.ReadValue());


		var testButton = new Button();
		testButton = new Button(()=> { test++; testButton.text = test.ToString() + "\n" + Pointer.current.position.ReadValue(); });
		testButton.style.width = 200;
		testButton.style.height = 200;
		testButton.style.left = 0;
		testButton.style.color = Color.white;
		rootVisualElement.Add(testButton);
*/

		// Each editor window contains a root VisualElement object
		VisualElement root = rootVisualElement;

		// Import UXML
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/DragAndDrop/DragAndDropWindow.uxml");
		VisualElement labelFromUXML = visualTree.Instantiate();
		root.Add(labelFromUXML);

		// A stylesheet can be added to a VisualElement.
		// The style will be applied to the VisualElement and all of its children.
		var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/DragAndDrop/DragAndDropWindow.uss");

		DragAndDropManipulator manipulator = new(rootVisualElement.Q<VisualElement>("object"));
	}
}
