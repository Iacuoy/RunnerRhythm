/// Credit Mrs. YakaYocha 
/// Sourced from - https://www.youtube.com/channel/UCHp8LZ_0-iCvl-5pjHATsgw
/// Please donate: https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RJ8D9FRFQF9VS

using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace CHBase 
{
    [RequireComponent(typeof(CHScrollRect))]
    public class SnapElementVerticalScroller : MonoBehaviour
    {
        [Tooltip("Scrollable area (content of desired CHScrollRect)")]
        public RectTransform _scrollingPanel;
        [Tooltip("Elements to populate inside the scroller")]
        public GameObject[] _arrayOfElements;
        [Tooltip("Center display area (position of zoomed content)")]
        public RectTransform _center;
        [Tooltip("Select the item to be in center on start. (optional)")]
        public int StartingIndex = -1;
        [Tooltip("Button to go to the next page. (optional)")]
        public GameObject ScrollUpButton;
        [Tooltip("Button to go to the previous page. (optional)")]
        public GameObject ScrollDownButton;
        [Tooltip("Event fired when a specific item is clicked, exposes index number of item. (optional)")]
        public UnityEvent<int> ButtonClicked;
        
        // 缩放效果曲线
        public AnimationCurve ScaleLerpCurve;
        // 最大Snap力度
        public float MaxSnapForce = 0.2f;
        // Snap速度阈值
        public float MagnifySpeedThreshold = 200f;

        private float[] distReposition;
        private float[] distance;
        //private int elementsDistance;
        private int minElementsNum;
        private int elementLength;
        //private int elementHalfLength;
        private float deltaY;
        private string result;

        [SerializeField]
        private ScrollerEvent m_OnValueChanged = new ScrollerEvent();
        
        private int _resultIdx;
        private CHScrollRect _scrollRect;
        // 上一帧位置，用于计算速度
        private float _lastScrollPosition;

        public int ResultIdx
        {
            get { return _resultIdx; }
        }
        public ScrollerEvent onValueChanged
        {
            get
            {
                return this.m_OnValueChanged;
            }
            set
            {
                this.m_OnValueChanged = value;
            }
        }
        
        public SnapElementVerticalScroller() { }

        public SnapElementVerticalScroller(RectTransform scrollingPanel, GameObject[] arrayOfElements, RectTransform center)
        {
            _scrollingPanel = scrollingPanel;
            _arrayOfElements = arrayOfElements;
            _center = center;
        }


        public void Awake()
        {
            _scrollRect = GetComponent<CHScrollRect>();
            if (!_scrollingPanel)
            {
                _scrollingPanel = _scrollRect.content;
            }
            if (!_center)
            {
                Debug.LogError("Please define the RectTransform for the Center viewport of the scrollable area");
            }
            if (_arrayOfElements == null || _arrayOfElements.Length == 0)
            {
                var childCount = _scrollRect.content.childCount;
                if (childCount > 0)
                {
                    _arrayOfElements = new GameObject[childCount];
                    for (int i = 0; i < childCount; i++)
                    {
                        _arrayOfElements[i] = _scrollRect.content.GetChild(i).gameObject;
                    }                    
                }
            }
        }

        public void Start()
        {
            if (_arrayOfElements.Length < 1)
            {
                Debug.Log("No child content found, exiting..");
                return;
            }

            elementLength = _arrayOfElements.Length;
            distance = new float[elementLength];
            distReposition = new float[elementLength];

            //get distance between buttons
            //elementsDistance = (int)Mathf.Abs(_arrayOfElements[1].GetComponent<RectTransform>().anchoredPosition.y - _arrayOfElements[0].GetComponent<RectTransform>().anchoredPosition.y);
            deltaY = _arrayOfElements[0].GetComponent<RectTransform>().rect.height * elementLength / 3 * 2;
            Vector2 startPosition = new Vector2(_scrollingPanel.anchoredPosition.x, -deltaY);
            _scrollingPanel.anchoredPosition = startPosition;

            for (var i = 0; i < _arrayOfElements.Length; i++)
            {
                AddListener(_arrayOfElements[i], i);
            }

            if (ScrollUpButton)
                ScrollUpButton.GetComponent<Button>().onClick.AddListener(() => { ScrollUp(); });

            if (ScrollDownButton)
                ScrollDownButton.GetComponent<Button>().onClick.AddListener(() => { ScrollDown(); });

            if (StartingIndex > -1)
            {
                StartingIndex = StartingIndex > _arrayOfElements.Length ? _arrayOfElements.Length - 1 : StartingIndex;
                SnapToElement(StartingIndex);
            }
        }

        private void AddListener(GameObject button, int index)
        {
            var btn = button.GetComponent<Button>();
            if (null != btn)
                btn.onClick.AddListener(() => DoSomething(index));
        }

        private void DoSomething(int index)
        {
            if (ButtonClicked != null)
            {
                ButtonClicked.Invoke(index);
            }
        }

        public void Update()
        {
            if (_arrayOfElements.Length < 1)
            {
                return;
            }

            for (var i = 0; i < elementLength; i++)
            {
                distReposition[i] = _center.GetComponent<RectTransform>().position.y - _arrayOfElements[i].GetComponent<RectTransform>().position.y;
                distance[i] = Mathf.Abs(distReposition[i]);
                // CH
                var maxDistance = (transform as RectTransform).sizeDelta.y * 0.5f +
                                  (_arrayOfElements[i].transform as RectTransform).sizeDelta.y * 0.5f;
                float scale = ScaleLerpCurve.Evaluate(1 - Mathf.Min(distance[i], maxDistance) / maxDistance);
                //Magnifying effect
//                float scale = Mathf.Max(0.7f, 1 / (1 + distance[i] / 200));
                _arrayOfElements[i].GetComponent<RectTransform>().transform.localScale = new Vector3(scale, scale, 1f);
            }
            float minDistance = Mathf.Min(distance);
            for (var i = 0; i < elementLength; i++)
            {
                _arrayOfElements[i].GetComponent<CanvasGroup>().interactable = false;
                if (minDistance == distance[i])
                {
                    minElementsNum = i;
                    _arrayOfElements[i].GetComponent<CanvasGroup>().interactable = true;
                    result = _arrayOfElements[i].GetComponentInChildren<Text>().text;
                    if (i != _resultIdx)
                    {
                        _resultIdx = i;
                        m_OnValueChanged.Invoke(_resultIdx);
                    }
                }
            }

            ScrollingElements(-_arrayOfElements[minElementsNum].GetComponent<RectTransform>().anchoredPosition.y, minDistance);
        }

        private void ScrollingElements(float position, float minDistance)
        {
            if (_scrollRect.Dragging)
            {
                _lastScrollPosition = _scrollingPanel.anchoredPosition.y;
                return;
            }
            var scrollSpeed = Mathf.Abs(_scrollingPanel.anchoredPosition.y - _lastScrollPosition) / Time.deltaTime;
            if (scrollSpeed < MagnifySpeedThreshold)
            {
                if (minDistance < 1f)
                {
                    _scrollingPanel.anchoredPosition = new Vector2(_scrollingPanel.anchoredPosition.x, position);
                    _lastScrollPosition = position;
                    _scrollRect.StopMovement();
                }
                else
                {
                    var force = Mathf.Lerp(MaxSnapForce * .0f, MaxSnapForce,
                        (MagnifySpeedThreshold - scrollSpeed) / MagnifySpeedThreshold);
                    float newY = Mathf.Lerp(_scrollingPanel.anchoredPosition.y, position, force);
                    Vector2 newPosition = new Vector2(_scrollingPanel.anchoredPosition.x, newY);
                    _scrollingPanel.anchoredPosition = newPosition;
                    _lastScrollPosition = newY;
                }
            }
            else
            {
                _lastScrollPosition = _scrollingPanel.anchoredPosition.y;    
            }
        }

        public string GetResults()
        {
            return result;
        }

        public void SnapToElement(int element)
        {
            float deltaElementPositionY = _arrayOfElements[0].GetComponent<RectTransform>().rect.height * element;
            Vector2 newPosition = new Vector2(_scrollingPanel.anchoredPosition.x, -deltaElementPositionY);
            _scrollingPanel.anchoredPosition = newPosition;

        }

        public void ScrollUp()
        {
            float deltaUp = _arrayOfElements[0].GetComponent<RectTransform>().rect.height / 1.2f;
            Vector2 newPositionUp = new Vector2(_scrollingPanel.anchoredPosition.x, _scrollingPanel.anchoredPosition.y - deltaUp);
            _scrollingPanel.anchoredPosition = Vector2.Lerp(_scrollingPanel.anchoredPosition, newPositionUp, 1);
        }

        public void ScrollDown()
        {
            float deltaDown = _arrayOfElements[0].GetComponent<RectTransform>().rect.height / 1.2f;
            Vector2 newPositionDown = new Vector2(_scrollingPanel.anchoredPosition.x, _scrollingPanel.anchoredPosition.y + deltaDown);
            _scrollingPanel.anchoredPosition = newPositionDown;
        }
        
        [System.Serializable]
        public class ScrollerEvent : UnityEvent<int>
        {
        }
    }
}