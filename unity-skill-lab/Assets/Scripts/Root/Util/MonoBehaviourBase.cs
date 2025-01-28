using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UnityEngine;

namespace Root.Util
{
    /*
     * BaseMonoBehaviour 클래스 개요
     * - Unity에서 MonoBehaviour 객체는 C++ 객체(gameObject)와 C# 객체(MonoBehaviour)로 나뉘어 관리되며,
     *   C# 객체는 가비지 컬렉션(GC)에 의해 수명이 관리된다.
     * - MonoBehaviour 객체가 파괴되더라도 해당 객체가 참조하고 있는 모든 레퍼런스가 해제되지 않으면
     *   GC가 이를 제거하지 못하고 메모리 상에 남아 있게 된다. 이를 "Ghost Reference" 문제라고 한다.
     * - 이러한 문제는 레퍼런스 그래프의 비대화와 게임 퍼포먼스 저하(틱 불안정)를 초래할 수 있다.
     * - 이를 해결하기 위해 OnDestroy에서 모든 멤버 변수의 참조를 명시적으로 null로 설정하거나 컬렉션을 비우는 작업을 수행해야 한다.
     *
     * [어떻게 동작하는지]
     * - `OnDestroy` 메서드는 Reflection을 활용하여 현재 클래스의 모든 필드를 순회하며, 메모리 관리를 최적화하기 위해 다음 작업을 수행한다.
     *   1. 모든 필드를 가져와(`FieldInfo`) 필드 타입을 확인한다.
     *   2. 값 타입(`Primitive Type`)은 GC 대상이 아니므로 별도 처리 없이 건너뛴다.
     *   3. 컬렉션 타입(`IList`, `IDictionary`, `Queue`, `Stack`, `HashSet`)인 경우, `Clear()` 메서드를 호출하여 내부 데이터를 제거한다.
     *   4. 그 외의 참조 타입(클래스 객체 등)은 `null`을 할당하여 GC가 해당 객체를 해제할 수 있도록 한다.
     */
    public abstract class BaseMonoBehaviour : MonoBehaviour
    {
        [SuppressMessage("Reflection", "S3011", Justification = 
            "필드의 메모리 해제를 자동화하기 위해 BindingFlags.NonPublic을 사용해야 함. " +
            "MonoBehaviour의 필드들은 private, protected, internal인 경우가 많아, " +
            "이를 포함하지 않으면 GC 해제가 완전하지 않을 수 있음.")]
        protected virtual void OnDestroy()
        {
            // 현재 클래스의 모든 필드 가져오기
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                // 값 타입(Primitive Type)은 처리할 필요 없음 → continue
                if (field.FieldType.IsValueType) continue;
                
                var fieldValue = field.GetValue(this);
                switch (fieldValue)
                {
                    case null:
                        continue;
                    
                    case IList list:
                        list.Clear();
                        break;
                    
                    case IDictionary dictionary:
                        dictionary.Clear();
                        break;
                    
                    case Queue queue:
                        queue.Clear();
                        break;
                    
                    case Stack stack:
                        stack.Clear();
                        break;
                    
                    case HashSet<object> hashSet:
                        hashSet.Clear();
                        break;
                }

                // 참조 해제하여 GC가 즉시 정리할 수 있도록 함
                field.SetValue(this, null);
            }
        }
    }
}
