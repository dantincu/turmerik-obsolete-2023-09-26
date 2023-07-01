using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Reflection.Cache;
using Turmerik.Reflection;
using Turmerik.Utils;
using Microsoft.Extensions.DependencyInjection;
using Turmerik.MathH;
using System.Collections.ObjectModel;
using Turmerik.Collections;

namespace Turmerik.LocalDevice.ReflectionCacheUnitTests
{
    public partial class FiltersEqComprUnitTest : UnitTestBase
    {
        private static readonly ReadOnlyCollection<bool> boolValues;
        private static readonly ReadOnlyCollection<MemberScope> allMemberScopes;
        private static readonly ReadOnlyCollection<MemberVisibility> allMemberVisibilities;
        private static readonly ReadOnlyCollection<MemberVisibility?> allNllblMemberVisibilities;
        private static readonly ReadOnlyCollection<FieldType> allFieldTypes;

        private readonly IMatrixBuilderFactory matrixBuilderFactory;
        private readonly IMatrixBuilder<MemberScope, MemberVisibility> methodAccessFilterTuplesBuilder;
        private readonly IMatrixBuilder<MemberScope, MemberVisibility, FieldType> fieldAccessFilterTuplesBuilder;
        private readonly IMatrixBuilder<MemberScope, bool, bool, MemberVisibility?, MemberVisibility?> propertyAccessFilterTuplesBuilder;
        private readonly IMatrixBuilder<MemberVisibility?, MemberVisibility?, MemberVisibility?> eventAccessFilterTuplesBuilder;

        private readonly MethodAccessibilityFilter[] allMethodAccessFilters;
        private readonly FieldAccessibilityFilter[] allFieldAccessFilters;
        private readonly EventAccessibilityFilter[] allEventAccessFilters;
        private readonly PropertyAccessibilityFilter[] allPropertyAccessFilters;

        static FiltersEqComprUnitTest()
        {
            boolValues = false.Arr(true).RdnlC();
            allMemberScopes = Enum.GetValues<MemberScope>().RdnlC();
            allMemberVisibilities = Enum.GetValues<MemberVisibility>().RdnlC();
            allNllblMemberVisibilities = allMemberVisibilities.Cast<MemberVisibility?>().RdnlC();
            allFieldTypes = Enum.GetValues<FieldType>().RdnlC();
        }

        public FiltersEqComprUnitTest()
        {
            matrixBuilderFactory = ServiceProvider.GetRequiredService<IMatrixBuilderFactory>();
            methodAccessFilterTuplesBuilder = matrixBuilderFactory.Create2D<MemberScope, MemberVisibility>();
            fieldAccessFilterTuplesBuilder = matrixBuilderFactory.Create3D<MemberScope, MemberVisibility, FieldType>();
            propertyAccessFilterTuplesBuilder = matrixBuilderFactory.Create5D<MemberScope, bool, bool, MemberVisibility?, MemberVisibility?>();
            eventAccessFilterTuplesBuilder = matrixBuilderFactory.Create3D<MemberVisibility?, MemberVisibility?, MemberVisibility?>();

            allMethodAccessFilters = methodAccessFilterTuplesBuilder.Generate(
                allMemberScopes,
                allMemberVisibilities,
                tuple => true).Select(
                    CreateMethodAccessibilityFilter).ToArray();

            allFieldAccessFilters = fieldAccessFilterTuplesBuilder.Generate(
                allMemberScopes,
                allMemberVisibilities,
                allFieldTypes,
                tuple => true).Select(
                    CreateFieldAccessibilityFilter).ToArray();

            allEventAccessFilters = eventAccessFilterTuplesBuilder.Generate(
                allNllblMemberVisibilities,
                allNllblMemberVisibilities,
                allNllblMemberVisibilities,
                tuple => true).Select(
                    CreateEventAccessibilityFilter).ToArray();

            allPropertyAccessFilters = propertyAccessFilterTuplesBuilder.Generate(
                allMemberScopes,
                boolValues,
                boolValues,
                allNllblMemberVisibilities,
                allNllblMemberVisibilities,
                tuple => true).Select(
                    CreatePropertyAccessibilityFilter).ToArray();
        }

        [Fact]
        public void MainMethodsTest()
        {
            PerformMethodTest(allMethodAccessFilters);
        }

        [Fact]
        public void MainEventsTest()
        {
            PerformEventTest(allEventAccessFilters);
        }

        [Fact]
        public void MainFieldsTest()
        {
            PerformFieldTest(allFieldAccessFilters);
        }

        [Fact]
        public void MainPropertiesTest()
        {
            PerformPropertyTest(allPropertyAccessFilters);
        }

        private void PerformTest<TFilter>(
            TFilter[] valuesArr,
            IEqualityComparer<TFilter> eqCompr)
        {
            for (int i = 0; i < valuesArr.Length; i++)
            {
                for (int j = 0; j < valuesArr.Length; j++)
                {
                    bool areEqual = eqCompr.Equals(
                        valuesArr[i],
                        valuesArr[j]);

                    bool isValid = areEqual == (i == j);
                    Assert.True(isValid);
                }
            }
        }

        private void PerformMethodTest(
            MethodAccessibilityFilter[] valuesArr) => PerformTest(
                valuesArr,
                MethodAccessFilterEqCompr);

        private void PerformFieldTest(
            FieldAccessibilityFilter[] valuesArr) => PerformTest(
                valuesArr,
                FieldAccessFilterEqCompr);

        private void PerformEventTest(
            EventAccessibilityFilter[] valuesArr) => PerformTest(
                valuesArr,
                EventAccessFilterEqCompr);

        private void PerformPropertyTest(
            PropertyAccessibilityFilter[] valuesArr) => PerformTest(
                valuesArr,
                PropAccessFilterEqCompr);

        private MethodAccessibilityFilter CreateMethodAccessibilityFilter(
            Tuple<MemberScope, MemberVisibility> tuple) => new MethodAccessibilityFilter(
                tuple.Item1,
                tuple.Item2);

        private FieldAccessibilityFilter CreateFieldAccessibilityFilter(
            Tuple<MemberScope, MemberVisibility, FieldType> tuple) => new FieldAccessibilityFilter(
                tuple.Item1,
                tuple.Item2,
                tuple.Item3);

        private EventAccessibilityFilter CreateEventAccessibilityFilter(
            Tuple<MemberVisibility?, MemberVisibility?, MemberVisibility?> tuple) => new EventAccessibilityFilter(
                CreateMethodAccessibilityFilter(tuple.Item1),
                CreateMethodAccessibilityFilter(tuple.Item2),
                CreateMethodAccessibilityFilter(tuple.Item3));

        private PropertyAccessibilityFilter CreatePropertyAccessibilityFilter(
            Tuple<MemberScope, bool, bool, MemberVisibility?, MemberVisibility?> tuple) => new PropertyAccessibilityFilter(
                tuple.Item1,
                tuple.Item2,
                tuple.Item3,
                CreateMethodAccessibilityFilter(tuple.Item4),
                CreateMethodAccessibilityFilter(tuple.Item5));

        private MethodAccessibilityFilter? CreateMethodAccessibilityFilter(
            MemberVisibility? memberVisibility) => memberVisibility?.WithValue(
                value => new MethodAccessibilityFilter(MemberScope.Instance, value));
    }
}
