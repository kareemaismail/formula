//-----------------------------------------------------------------------
// <copyright file="TestCsWrapper.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Formula.Parser.CsTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Formula.Parser;
    using Formula.Parser.Integration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestCsWrapper : TestBase
    {
        [TestMethod]
        public void TestInterpetUsage()
        {
            var input = "42";

            var result = CsWrapper.InterpretFormula(input);
            Assert.AreEqual(42, result);

            var ast = CsWrapper.ParseFormula(input);
            result = CsWrapper.InterpretExpression(ast);
            Assert.AreEqual(42, result);

            var folded = CsWrapper.ConstantFoldExpression(ast);
            result = CsWrapper.InterpretExpression(folded);
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void TestInterpeterDepth()
        {
#if DEBUG
            Console.WriteLine("DEBUG");
#else
            Console.WriteLine("RELEASE");
#endif
            var depth = 500;
            var function = "SQRT[Test]";
            var input = new StringBuilder();

            for (int i = 0; i < depth; i++)
            {
                input.Append("(");
            }
            input.Append($"{function})");
            for (int i = 0; i < depth - 1; i++)
            {
                input.Append($"* {function})");
            }

            var inputStr = input.ToString();
            var sw = Stopwatch.StartNew();
            var result = CsWrapper.InterpretFormula(inputStr, new MapVariableProvider(new Dictionary<string, double>() { { "Test", 1 } }), DefaultFunctionProvider.Instance);
            sw.Stop();

            Console.WriteLine($"Depth: {depth}, Time: {sw.ElapsedMilliseconds}ms");

            Assert.AreEqual(1, result);
        }

        class CustomFunctionProvider: IFunctionProvider
        {
            class MyFuncImplementation : IFunctionImplementation
            {
                public double Execute(double[] input) => 42.0;

                public bool Validate(double[] input, out string message)
                {
                    if (input.Length == 0)
                    {
                        message = String.Empty;
                        return true;
                    }
                    else
                    {
                        message = "Expected no arguments";
                        return false;
                    }
                }

                public string Name => "MyFunc";
            }

            public IEnumerable<string> KnownFunctions => new[] { "MyFunc" };

            public bool IsDefined(string name) => name == "MyFunc";

            public IFunctionImplementation Lookup(string name)
            {
                if (name == "MyFunc")
                {
                    return new MyFuncImplementation();
                }

                return null;
            }
        }

        [TestMethod]
        public void TestCustomFunctionProvider()
        {
            var input = "MyFunc[]";

            var result = CsWrapper.InterpretFormula(input, new CustomFunctionProvider());

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void TestCompositeFunctionProvider()
        {
            var input = "MyFunc[] * SQRT[4]";

            var result = CsWrapper.InterpretFormula(input, new CompositeFunctionProvider(new [] {new CustomFunctionProvider(), DefaultFunctionProvider.Instance }));

            Assert.AreEqual(84, result);
        }

        [TestMethod]
        public void TestExpressionVariableProvider()
        {
            var input = new Dictionary<string, string>()
            {
                { "A", "B*C" },
                { "B", "C * 10" },
                { "C", "SQRT[4] * 5" }
            };

            var variableProvider = new ExpressionVariableProvider(input, DefaultFunctionProvider.Instance);

            var result = variableProvider.Lookup("A");

            Assert.AreEqual(1000, result);
        }

        [TestMethod]
        public void TestExpressionVariableProviderWithLargeVolume()
        {
            var input = new Dictionary<string, string>()
            {
                {"AverageStoreSize_0", "10429.548"},
                {"AverageStoreSize_1", "10429.548"},
                {"AverageStoreSize_2", "10429.548"},
                {"AverageStoreSize_3", "10429.548"},
                {"AverageStoreSize_4", "10429.548"},
                {"AverageStoreSize_5", "10429.548"},
                {"AverageStoreSize_6", "10429.548"},
                {"AverageStoreSize_7", "10429.548"},
                {"AverageStoreSize_8", "10429.548"},
                {"AverageStoreSize_9", "10429.548"},
                {"AverageStoreSize_10", "10429.548"},
                {"AverageStoreSize_11", "10429.548"},
                {"AverageStoreSize_12", "10429.548"},
                {"AverageStoreSize_13", "10429.548"},
                {"AverageStoreSize_14", "10429.548"},
                {"AverageStoreSize_15", "10429.548"},
                {"TradingDensity_0", "17960.2077381174"},
                {"TradingDensity_1", "17960.2077381174"},
                {"TradingDensity_2", "17960.2077381174"},
                {"TradingDensity_3", "17960.2077381174"},
                {"TradingDensity_4", "17960.2077381174"},
                {"TradingDensity_5", "17960.2077381174"},
                {"TradingDensity_6", "17960.2077381174"},
                {"TradingDensity_7", "17960.2077381174"},
                {"TradingDensity_8", "17960.2077381174"},
                {"TradingDensity_9", "17960.2077381174"},
                {"TradingDensity_10", "17960.2077381174"},
                {"TradingDensity_11", "17960.2077381174"},
                {"TradingDensity_12", "17960.2077381174"},
                {"TradingDensity_13", "17960.2077381174"},
                {"TradingDensity_14", "17960.2077381174"},
                {"TradingDensity_15", "17960.2077381174"},
                {"TradingDensityFactor_0", "0"},
                {"TradingDensityFactor_1", "0"},
                {"TradingDensityFactor_2", "0"},
                {"TradingDensityFactor_3", "0"},
                {"TradingDensityFactor_4", "0"},
                {"TradingDensityFactor_5", "0"},
                {"TradingDensityFactor_6", "0"},
                {"TradingDensityFactor_7", "0"},
                {"TradingDensityFactor_8", "0"},
                {"TradingDensityFactor_9", "0"},
                {"TradingDensityFactor_10", "0"},
                {"TradingDensityFactor_11", "0"},
                {"TradingDensityFactor_12", "0"},
                {"TradingDensityFactor_13", "0"},
                {"TradingDensityFactor_14", "0"},
                {"TradingDensityFactor_15", "0"},
                {"CurrentNetStores_0", "15"},
                {"CurrentNetStores_1", "15"},
                {"CurrentNetStores_2", "15"},
                {"CurrentNetStores_3", "15"},
                {"CurrentNetStores_4", "15"},
                {"CurrentNetStores_5", "15"},
                {"CurrentNetStores_6", "15"},
                {"CurrentNetStores_7", "15"},
                {"CurrentNetStores_8", "15"},
                {"CurrentNetStores_9", "15"},
                {"CurrentNetStores_10", "15"},
                {"CurrentNetStores_11", "15"},
                {"CurrentNetStores_12", "15"},
                {"CurrentNetStores_13", "15"},
                {"CurrentNetStores_14", "15"},
                {"CurrentNetStores_15", "15"},
                {"MarkdownsFactor_0", "0.4"},
                {"MarkdownsFactor_1", "0.4"},
                {"MarkdownsFactor_2", "0.4"},
                {"MarkdownsFactor_3", "0.4"},
                {"MarkdownsFactor_4", "0.4"},
                {"MarkdownsFactor_5", "0.4"},
                {"MarkdownsFactor_6", "0.4"},
                {"MarkdownsFactor_7", "0.4"},
                {"MarkdownsFactor_8", "0.4"},
                {"MarkdownsFactor_9", "0.4"},
                {"MarkdownsFactor_10", "0.4"},
                {"MarkdownsFactor_11", "0.4"},
                {"MarkdownsFactor_12", "0.4"},
                {"MarkdownsFactor_13", "0.4"},
                {"MarkdownsFactor_14", "0.4"},
                {"MarkdownsFactor_15", "0.4"},
                {"PromotionsFactor_0", "0.6"},
                {"PromotionsFactor_1", "0.6"},
                {"PromotionsFactor_2", "0.6"},
                {"PromotionsFactor_3", "0.6"},
                {"PromotionsFactor_4", "0.6"},
                {"PromotionsFactor_5", "0.6"},
                {"PromotionsFactor_6", "0.6"},
                {"PromotionsFactor_7", "0.6"},
                {"PromotionsFactor_8", "0.6"},
                {"PromotionsFactor_9", "0.6"},
                {"PromotionsFactor_10", "0.6"},
                {"PromotionsFactor_11", "0.6"},
                {"PromotionsFactor_12", "0.6"},
                {"PromotionsFactor_13", "0.6"},
                {"PromotionsFactor_14", "0.6"},
                {"PromotionsFactor_15", "0.6"},
                {"MarkdownsPercentOfHyperionGrossSales_0", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_1", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_2", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_3", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_4", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_5", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_6", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_7", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_8", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_9", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_10", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_11", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_12", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_13", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_14", "0.03"},
                {"MarkdownsPercentOfHyperionGrossSales_15", "0.03"},
                {"OpenedStoreCannibilisationFactor_0", "0.15"},
                {"OpenedStoreCannibilisationFactor_1", "0.15"},
                {"OpenedStoreCannibilisationFactor_2", "0.15"},
                {"OpenedStoreCannibilisationFactor_3", "0.15"},
                {"OpenedStoreCannibilisationFactor_4", "0.15"},
                {"OpenedStoreCannibilisationFactor_5", "0.15"},
                {"OpenedStoreCannibilisationFactor_6", "0.15"},
                {"OpenedStoreCannibilisationFactor_7", "0.15"},
                {"OpenedStoreCannibilisationFactor_8", "0.15"},
                {"OpenedStoreCannibilisationFactor_9", "0.15"},
                {"OpenedStoreCannibilisationFactor_10", "0.15"},
                {"OpenedStoreCannibilisationFactor_11", "0.15"},
                {"OpenedStoreCannibilisationFactor_12", "0.15"},
                {"OpenedStoreCannibilisationFactor_13", "0.15"},
                {"OpenedStoreCannibilisationFactor_14", "0.15"},
                {"OpenedStoreCannibilisationFactor_15", "0.15"},
                {"ClosedStoreCannibilisationFactor_0", "0.7"},
                {"ClosedStoreCannibilisationFactor_1", "0.7"},
                {"ClosedStoreCannibilisationFactor_2", "0.7"},
                {"ClosedStoreCannibilisationFactor_3", "0.7"},
                {"ClosedStoreCannibilisationFactor_4", "0.7"},
                {"ClosedStoreCannibilisationFactor_5", "0.7"},
                {"ClosedStoreCannibilisationFactor_6", "0.7"},
                {"ClosedStoreCannibilisationFactor_7", "0.7"},
                {"ClosedStoreCannibilisationFactor_8", "0.7"},
                {"ClosedStoreCannibilisationFactor_9", "0.7"},
                {"ClosedStoreCannibilisationFactor_10", "0.7"},
                {"ClosedStoreCannibilisationFactor_11", "0.7"},
                {"ClosedStoreCannibilisationFactor_12", "0.7"},
                {"ClosedStoreCannibilisationFactor_13", "0.7"},
                {"ClosedStoreCannibilisationFactor_14", "0.7"},
                {"ClosedStoreCannibilisationFactor_15", "0.7"},
                {"NumberOfOpenedStores_0", "0"},
                {"NumberOfOpenedStores_1", "0"},
                {"NumberOfOpenedStores_2", "0"},
                {"NumberOfOpenedStores_3", "0"},
                {"NumberOfOpenedStores_4", "0"},
                {"NumberOfOpenedStores_5", "0"},
                {"NumberOfOpenedStores_6", "0"},
                {"NumberOfOpenedStores_7", "0"},
                {"NumberOfOpenedStores_8", "0"},
                {"NumberOfOpenedStores_9", "0"},
                {"NumberOfOpenedStores_10", "0"},
                {"NumberOfOpenedStores_11", "0"},
                {"NumberOfOpenedStores_12", "0"},
                {"NumberOfOpenedStores_13", "0"},
                {"NumberOfOpenedStores_14", "0"},
                {"NumberOfOpenedStores_15", "0"},
                {"NumberOfClosedStores_0", "0"},
                {"NumberOfClosedStores_1", "0"},
                {"NumberOfClosedStores_2", "0"},
                {"NumberOfClosedStores_3", "0"},
                {"NumberOfClosedStores_4", "0"},
                {"NumberOfClosedStores_5", "0"},
                {"NumberOfClosedStores_6", "0"},
                {"NumberOfClosedStores_7", "0"},
                {"NumberOfClosedStores_8", "0"},
                {"NumberOfClosedStores_9", "0"},
                {"NumberOfClosedStores_10", "0"},
                {"NumberOfClosedStores_11", "0"},
                {"NumberOfClosedStores_12", "0"},
                {"NumberOfClosedStores_13", "0"},
                {"NumberOfClosedStores_14", "0"},
                {"NumberOfClosedStores_15", "0"},
                {"NumberOfSoldStores_0", "0"},
                {"NumberOfSoldStores_1", "0"},
                {"NumberOfSoldStores_2", "0"},
                {"NumberOfSoldStores_3", "0"},
                {"NumberOfSoldStores_4", "0"},
                {"NumberOfSoldStores_5", "0"},
                {"NumberOfSoldStores_6", "0"},
                {"NumberOfSoldStores_7", "0"},
                {"NumberOfSoldStores_8", "0"},
                {"NumberOfSoldStores_9", "0"},
                {"NumberOfSoldStores_10", "0"},
                {"NumberOfSoldStores_11", "0"},
                {"NumberOfSoldStores_12", "0"},
                {"NumberOfSoldStores_13", "0"},
                {"NumberOfSoldStores_14", "0"},
                {"NumberOfSoldStores_15", "0"},
                {"NumberOfStoresConvertedAcrossFormats_0", "0"},
                {"NumberOfStoresConvertedAcrossFormats_1", "0"},
                {"NumberOfStoresConvertedAcrossFormats_2", "0"},
                {"NumberOfStoresConvertedAcrossFormats_3", "0"},
                {"NumberOfStoresConvertedAcrossFormats_4", "0"},
                {"NumberOfStoresConvertedAcrossFormats_5", "0"},
                {"NumberOfStoresConvertedAcrossFormats_6", "0"},
                {"NumberOfStoresConvertedAcrossFormats_7", "0"},
                {"NumberOfStoresConvertedAcrossFormats_8", "0"},
                {"NumberOfStoresConvertedAcrossFormats_9", "0"},
                {"NumberOfStoresConvertedAcrossFormats_10", "0"},
                {"NumberOfStoresConvertedAcrossFormats_11", "0"},
                {"NumberOfStoresConvertedAcrossFormats_12", "0"},
                {"NumberOfStoresConvertedAcrossFormats_13", "0"},
                {"NumberOfStoresConvertedAcrossFormats_14", "0"},
                {"NumberOfStoresConvertedAcrossFormats_15", "0"},
                {"AverageStoreEmployeeCosts_0", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_1", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_2", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_3", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_4", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_5", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_6", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_7", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_8", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_9", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_10", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_11", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_12", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_13", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_14", "16.3637476893333"},
                {"AverageStoreEmployeeCosts_15", "16.3637476893333"},
                {"AverageStoreRent_0", "0"},
                {"AverageStoreRent_1", "0"},
                {"AverageStoreRent_2", "0"},
                {"AverageStoreRent_3", "0"},
                {"AverageStoreRent_4", "0"},
                {"AverageStoreRent_5", "0"},
                {"AverageStoreRent_6", "0"},
                {"AverageStoreRent_7", "0"},
                {"AverageStoreRent_8", "0"},
                {"AverageStoreRent_9", "0"},
                {"AverageStoreRent_10", "0"},
                {"AverageStoreRent_11", "0"},
                {"AverageStoreRent_12", "0"},
                {"AverageStoreRent_13", "0"},
                {"AverageStoreRent_14", "0"},
                {"AverageStoreRent_15", "0"},
                {"CapexDepreciation_0", "0"},
                {"CapexDepreciation_1", "0"},
                {"CapexDepreciation_2", "0"},
                {"CapexDepreciation_3", "0"},
                {"CapexDepreciation_4", "0"},
                {"CapexDepreciation_5", "0"},
                {"CapexDepreciation_6", "0"},
                {"CapexDepreciation_7", "0"},
                {"CapexDepreciation_8", "0"},
                {"CapexDepreciation_9", "0"},
                {"CapexDepreciation_10", "0"},
                {"CapexDepreciation_11", "0"},
                {"CapexDepreciation_12", "0"},
                {"CapexDepreciation_13", "0"},
                {"CapexDepreciation_14", "0"},
                {"CapexDepreciation_15", "0"},
                {"ITCostsPercentage_0", "0.034"},
                {"ITCostsPercentage_1", "0.034"},
                {"ITCostsPercentage_2", "0.034"},
                {"ITCostsPercentage_3", "0.034"},
                {"ITCostsPercentage_4", "0.034"},
                {"ITCostsPercentage_5", "0.034"},
                {"ITCostsPercentage_6", "0.034"},
                {"ITCostsPercentage_7", "0.034"},
                {"ITCostsPercentage_8", "0.034"},
                {"ITCostsPercentage_9", "0.034"},
                {"ITCostsPercentage_10", "0.034"},
                {"ITCostsPercentage_11", "0.034"},
                {"ITCostsPercentage_12", "0.034"},
                {"ITCostsPercentage_13", "0.034"},
                {"ITCostsPercentage_14", "0.034"},
                {"ITCostsPercentage_15", "0.034"},
                {"EmployeeWageInflationFactor_0", "0"},
                {"EmployeeWageInflationFactor_1", "0"},
                {"EmployeeWageInflationFactor_2", "0"},
                {"EmployeeWageInflationFactor_3", "0"},
                {"EmployeeWageInflationFactor_4", "0"},
                {"EmployeeWageInflationFactor_5", "0.05"},
                {"EmployeeWageInflationFactor_6", "0.05"},
                {"EmployeeWageInflationFactor_7", "0.05"},
                {"EmployeeWageInflationFactor_8", "0.05"},
                {"EmployeeWageInflationFactor_9", "0.05"},
                {"EmployeeWageInflationFactor_10", "0.05"},
                {"EmployeeWageInflationFactor_11", "0.05"},
                {"EmployeeWageInflationFactor_12", "0.05"},
                {"EmployeeWageInflationFactor_13", "0.05"},
                {"EmployeeWageInflationFactor_14", "0.05"},
                {"EmployeeWageInflationFactor_15", "0.05"},
                {"RentalInflationFactor_0", "0"},
                {"RentalInflationFactor_1", "0"},
                {"RentalInflationFactor_2", "0"},
                {"RentalInflationFactor_3", "0"},
                {"RentalInflationFactor_4", "0"},
                {"RentalInflationFactor_5", "0.07"},
                {"RentalInflationFactor_6", "0.07"},
                {"RentalInflationFactor_7", "0.07"},
                {"RentalInflationFactor_8", "0.07"},
                {"RentalInflationFactor_9", "0.07"},
                {"RentalInflationFactor_10", "0.07"},
                {"RentalInflationFactor_11", "0.07"},
                {"RentalInflationFactor_12", "0.07"},
                {"RentalInflationFactor_13", "0.07"},
                {"RentalInflationFactor_14", "0.07"},
                {"RentalInflationFactor_15", "0.07"},
                {"StoreCapexInflationFactor_0", "0"},
                {"StoreCapexInflationFactor_1", "0"},
                {"StoreCapexInflationFactor_2", "0"},
                {"StoreCapexInflationFactor_3", "0"},
                {"StoreCapexInflationFactor_4", "0"},
                {"StoreCapexInflationFactor_5", "0.07"},
                {"StoreCapexInflationFactor_6", "0.07"},
                {"StoreCapexInflationFactor_7", "0.07"},
                {"StoreCapexInflationFactor_8", "0.07"},
                {"StoreCapexInflationFactor_9", "0.07"},
                {"StoreCapexInflationFactor_10", "0.07"},
                {"StoreCapexInflationFactor_11", "0.07"},
                {"StoreCapexInflationFactor_12", "0.07"},
                {"StoreCapexInflationFactor_13", "0.07"},
                {"StoreCapexInflationFactor_14", "0.07"},
                {"StoreCapexInflationFactor_15", "0.07"},
                {"SalesGrowthFactor_0", "0"},
                {"SalesGrowthFactor_1", "0"},
                {"SalesGrowthFactor_2", "0"},
                {"SalesGrowthFactor_3", "0"},
                {"SalesGrowthFactor_4", "0"},
                {"SalesGrowthFactor_5", "0.07"},
                {"SalesGrowthFactor_6", "0.07"},
                {"SalesGrowthFactor_7", "0.07"},
                {"SalesGrowthFactor_8", "0.07"},
                {"SalesGrowthFactor_9", "0.07"},
                {"SalesGrowthFactor_10", "0.07"},
                {"SalesGrowthFactor_11", "0.07"},
                {"SalesGrowthFactor_12", "0.07"},
                {"SalesGrowthFactor_13", "0.07"},
                {"SalesGrowthFactor_14", "0.07"},
                {"SalesGrowthFactor_15", "0.07"},
                {"InflationExponent_0", "1"},
                {"InflationExponent_1", "1"},
                {"InflationExponent_2", "1"},
                {"InflationExponent_3", "1"},
                {"InflationExponent_4", "1"},
                {"InflationExponent_5", "1"},
                {"InflationExponent_6", "2"},
                {"InflationExponent_7", "3"},
                {"InflationExponent_8", "4"},
                {"InflationExponent_9", "5"},
                {"InflationExponent_10", "6"},
                {"InflationExponent_11", "7"},
                {"InflationExponent_12", "8"},
                {"InflationExponent_13", "9"},
                {"InflationExponent_14", "10"},
                {"InflationExponent_15", "11"},
                {"CorporateTaxRate_0", "0.28"},
                {"CorporateTaxRate_1", "0.28"},
                {"CorporateTaxRate_2", "0.28"},
                {"CorporateTaxRate_3", "0.28"},
                {"CorporateTaxRate_4", "0.28"},
                {"CorporateTaxRate_5", "0.28"},
                {"CorporateTaxRate_6", "0.28"},
                {"CorporateTaxRate_7", "0.28"},
                {"CorporateTaxRate_8", "0.28"},
                {"CorporateTaxRate_9", "0.28"},
                {"CorporateTaxRate_10", "0.28"},
                {"CorporateTaxRate_11", "0.28"},
                {"CorporateTaxRate_12", "0.28"},
                {"CorporateTaxRate_13", "0.28"},
                {"CorporateTaxRate_14", "0.28"},
                {"CorporateTaxRate_15", "0.28"},
                {"NewStoreDensityFactorBoost_0", "0"},
                {"NewStoreDensityFactorBoost_1", "0"},
                {"NewStoreDensityFactorBoost_2", "0"},
                {"NewStoreDensityFactorBoost_3", "0"},
                {"NewStoreDensityFactorBoost_4", "0"},
                {"NewStoreDensityFactorBoost_5", "0"},
                {"NewStoreDensityFactorBoost_6", "0"},
                {"NewStoreDensityFactorBoost_7", "0"},
                {"NewStoreDensityFactorBoost_8", "0"},
                {"NewStoreDensityFactorBoost_9", "0"},
                {"NewStoreDensityFactorBoost_10", "0"},
                {"NewStoreDensityFactorBoost_11", "0"},
                {"NewStoreDensityFactorBoost_12", "0"},
                {"NewStoreDensityFactorBoost_13", "0"},
                {"NewStoreDensityFactorBoost_14", "0"},
                {"NewStoreDensityFactorBoost_15", "0"},
                {"HyperionGrossSales_0", "3304.97963626"},
                {
                    "NetStores_0",
                    "CurrentNetStores_0 + NumberOfOpenedStores_0 - NumberOfClosedStores_0 - NumberOfSoldStores_0 + NumberOfStoresConvertedAcrossFormats_0"
                },
                {"EmployeeWageInflation_0", "EmployeeWageInflation_-1 * (1 + EmployeeWageInflationFactor_0)"},
                {"RentalInflation_0", "RentalInflation_-1 * (1 + RentalInflationFactor_0)"},
                {"StoreCapexInflation_0", "StoreCapexInflation_-1 * (1 + StoreCapexInflationFactor_0)"},
                {"SalesGrowth_0", "1"},
                {
                    "ClosingCannibilisationEffect_0",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_0",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_0",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_0", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {"GrossSales_0", "3404.1290253478"},
                {"Markdowns_0", "-39.65975563512"},
                {"Promotions_0", "-59.48963345268"},
                {"LoyaltyDiscount_0", "-44.67187315"},
                {"CreditSales_0", "1271.2888979078"},
                {"CashSales_0", "NetSales - CreditSales"},
                {"TotalCostofSales_0", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_0", "RetekGrossProfit"},
                {"RetekGrossProfit_0", "1406.11230637"},
                {"IndirectCostofSales_0", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_0", "-47.6084852001123"},
                {"Rebates_0", "21.25848596"},
                {
                    "OtherAdjustments_0",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_0", "-0.04971429"},
                {"CentralStoreReturns_0", "-9.75094076"},
                {"SupplierAdjustments_0", "-0.68736579"},
                {"OtherGrossProfitAdjustments_0", "0"},
                {"ConcessionFee_0", "0"},
                {"Advertising_0", "-0.011"},
                {
                    "TotalExpenses_0",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_0", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_0",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_0", "-232.40094722"},
                {"Stocklosses_0", "-48.84128526"},
                {"Rent_0", "-193.14961104"},
                {"Creditcardcommisions_0", "-24.78304568"},
                {"Security_0", "-16.84093323"},
                {"Cleaning_0", "-4.64609469"},
                {"Printingandstationery_0", "-11.97363785"},
                {"Communications_0", "-3.81745246"},
                {"WaterAndelectricity_0", "-38.78582425"},
                {"RepairsAndmaintenance_0", "-16.83736707"},
                {"Other1_0", "-65.16561628"},
                {
                    "StoreNonCashCosts_0",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_0", "-6.58791585897302"},
                {"Onerousleasecredit_0", "0"},
                {"Depreciation1_0", "-65.73015318"},
                {"Amortisationofgoodwillandintangibleassets1_0", "0"},
                {"WriteoffofpropertyplantAndequipment_0", "0"},
                {"ABSAMerchantFee_0", "-69.6861815967761"},
                {"ClubProfit_0", "56.833743014934"},
                {"SharedServices_0", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_0",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_0", "-58.393125266153"},
                {"Depreciation2_0", "-0.0119793874867962"},
                {"Amortisationofgoodwillandintangibleassets_0", "0"},
                {"AssetDisposal1_0", "0"},
                {"Travel1_0", "-6.36671422567515"},
                {"Other2_0", "13.0045952524239"},
                {
                    "Chainmanagementmerchandisse_0",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_0", "-65.9295547515903"},
                {"Travel2_0", "-3.63295458149044"},
                {"Depreciation3_0", "-1.22229682316428"},
                {"Other3_0", "-17.3646204810113"},
                {"CapexDepreciation1_0", "0"},
                {"AssetDisposal2_0", "0"},
                {"CorporateOverheads_0", "Chainadministrativecosts"},
                {"Chainadministrativecosts_0", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_0", "0"},
                {"Other_0", "0.00951229002859489"},
                {"Allocations_0", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_0",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_0", "-160.24555224094"},
                {"HeadOfficeRentalSavings_0", "0"},
                {"ITCosts_0", "-110.85046394574"},
                {"Cellularallocations_0", "0"},
                {"CreditAndFSallocations_0", "130.59422645251"},
                {"GrossProfit_0", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_0",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_0", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_0",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_0",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_0", "-58.5088502992049"},
                {"Corporateamortisationofgoodwillandintangibleassets2_0", "0"},
                {"Interest_0", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_0", "0"},
                {"Financinginterest_0", "-67.1964275731378"},
                {"EBIT_0", "GrossProfit + TotalExpenses"},
                {"EBITDA_0", "EBIT - DepreciationandAmmortisation"},
                {"EBT_0", "EBIT + Interest"},
                {"Tax_0", "24.3485567292904"},
                {"NetEarnings_0", "EBT - Tax"},
                {"PercentofRevenue_0", "GrossProfit / NetSales"},
                {"HyperionGrossSales_1", "3232.43665545"},
                {
                    "NetStores_1",
                    "CurrentNetStores_1 + NumberOfOpenedStores_1 - NumberOfClosedStores_1 - NumberOfSoldStores_1 + NumberOfStoresConvertedAcrossFormats_1"
                },
                {"EmployeeWageInflation_1", "EmployeeWageInflation_0 * (1 + EmployeeWageInflationFactor_1)"},
                {"RentalInflation_1", "RentalInflation_0 * (1 + RentalInflationFactor_1)"},
                {"StoreCapexInflation_1", "StoreCapexInflation_0 * (1 + StoreCapexInflationFactor_1)"},
                {"SalesGrowth_1", "1"},
                {
                    "ClosingCannibilisationEffect_1",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_1",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_1",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_1", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {"GrossSales_1", "3329.4097551135"},
                {"Markdowns_1", "-38.7892398654"},
                {"Promotions_1", "-58.1838597981"},
                {"LoyaltyDiscount_1", "-33.92464897"},
                {"CreditSales_1", "1751.87038822807"},
                {"CashSales_1", "NetSales - CreditSales"},
                {"TotalCostofSales_1", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_1", "RetekGrossProfit"},
                {"RetekGrossProfit_1", "1412.14164239"},
                {"IndirectCostofSales_1", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_1", "-48.8562945121741"},
                {"Rebates_1", "16.83007023"},
                {
                    "OtherAdjustments_1",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_1", "-0.1111019"},
                {"CentralStoreReturns_1", "-11.81467875"},
                {"SupplierAdjustments_1", "2.25464985"},
                {"OtherGrossProfitAdjustments_1", "0"},
                {"ConcessionFee_1", "0"},
                {"Advertising_1", "-0.0073889"},
                {
                    "TotalExpenses_1",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_1", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_1",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_1", "-228.24504159"},
                {"Stocklosses_1", "-44.03401875"},
                {"Rent_1", "-200.95624777"},
                {"Creditcardcommisions_1", "-27.09107727"},
                {"Security_1", "-17.90387849"},
                {"Cleaning_1", "-4.56629532"},
                {"Printingandstationery_1", "-11.14845561"},
                {"Communications_1", "-3.88552708"},
                {"WaterAndelectricity_1", "-41.71825759"},
                {"RepairsAndmaintenance_1", "-12.40519042"},
                {"Other1_1", "-68.83424378"},
                {
                    "StoreNonCashCosts_1",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_1", "-11.3077385198837"},
                {"Onerousleasecredit_1", "0"},
                {"Depreciation1_1", "-80.09153729"},
                {"Amortisationofgoodwillandintangibleassets1_1", "0"},
                {"WriteoffofpropertyplantAndequipment_1", "-9.76839661"},
                {"ABSAMerchantFee_1", "-70.5485338552766"},
                {"ClubProfit_1", "57.7786029896571"},
                {"SharedServices_1", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_1",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_1", "-19.8119596573585"},
                {"Depreciation2_1", "-0.00051470432740575"},
                {"Amortisationofgoodwillandintangibleassets_1", "0"},
                {"AssetDisposal1_1", "0"},
                {"Travel1_1", "-3.10099750317521"},
                {"Other2_1", "-0.979481034181217"},
                {
                    "Chainmanagementmerchandisse_1",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_1", "-62.9703218103904"},
                {"Travel2_1", "-3.40379232078349"},
                {"Depreciation3_1", "-0.870628920385948"},
                {"Other3_1", "0.322164341899583"},
                {"CapexDepreciation1_1", "0"},
                {"AssetDisposal2_1", "0"},
                {"CorporateOverheads_1", "Chainadministrativecosts"},
                {"Chainadministrativecosts_1", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_1", "0"},
                {"Other_1", "-0.146391792342597"},
                {"Allocations_1", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_1",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_1", "-216.727821045188"},
                {"HeadOfficeRentalSavings_1", "0"},
                {"ITCosts_1", "-108.74940822032"},
                {"Cellularallocations_1", "0"},
                {"CreditAndFSallocations_1", "168.69223956293"},
                {"GrossProfit_1", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_1",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_1", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_1",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_1",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_1", "-48.3471608506603"},
                {"Corporateamortisationofgoodwillandintangibleassets2_1", "0"},
                {"Interest_1", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_1", "0"},
                {"Financinginterest_1", "-67.1964275731378"},
                {"EBIT_1", "GrossProfit + TotalExpenses"},
                {"EBITDA_1", "EBIT - DepreciationandAmmortisation"},
                {"EBT_1", "EBIT + Interest"},
                {"Tax_1", "29.1884130128351"},
                {"NetEarnings_1", "EBT - Tax"},
                {"PercentofRevenue_1", "GrossProfit / NetSales"},
                {"HyperionGrossSales_2", "3018.90599163018"},
                {
                    "NetStores_2",
                    "CurrentNetStores_2 + NumberOfOpenedStores_2 - NumberOfClosedStores_2 - NumberOfSoldStores_2 + NumberOfStoresConvertedAcrossFormats_2"
                },
                {"EmployeeWageInflation_2", "EmployeeWageInflation_1 * (1 + EmployeeWageInflationFactor_2)"},
                {"RentalInflation_2", "RentalInflation_1 * (1 + RentalInflationFactor_2)"},
                {"StoreCapexInflation_2", "StoreCapexInflation_1 * (1 + StoreCapexInflationFactor_2)"},
                {"SalesGrowth_2", "1"},
                {
                    "ClosingCannibilisationEffect_2",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_2",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_2",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_2", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {"GrossSales_2", "3109.47317137908"},
                {"Markdowns_2", "-36.2268718995621"},
                {"Promotions_2", "-54.3403078493432"},
                {"LoyaltyDiscount_2", "-26.66610087"},
                {"CreditSales_2", "1566.20109735965"},
                {"CashSales_2", "NetSales - CreditSales"},
                {"TotalCostofSales_2", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_2", "RetekGrossProfit"},
                {"RetekGrossProfit_2", "1299.72493457018"},
                {"IndirectCostofSales_2", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_2", "-42.5621749983786"},
                {"Rebates_2", "16.1292904"},
                {
                    "OtherAdjustments_2",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_2", "-0.0364875"},
                {"CentralStoreReturns_2", "-11.70266891"},
                {"SupplierAdjustments_2", "-1.29198473"},
                {"OtherGrossProfitAdjustments_2", "0"},
                {"ConcessionFee_2", "0"},
                {"Advertising_2", "0.07798128"},
                {
                    "TotalExpenses_2",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_2", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_2",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_2", "-213.24618893"},
                {"Stocklosses_2", "-25.1918080399999"},
                {"Rent_2", "-222.3320489"},
                {"Creditcardcommisions_2", "-20.08588466"},
                {"Security_2", "-21.81295814"},
                {"Cleaning_2", "-4.62350594"},
                {"Printingandstationery_2", "-11.00613125"},
                {"Communications_2", "-3.82335147"},
                {"WaterAndelectricity_2", "-41.95928686"},
                {"RepairsAndmaintenance_2", "-11.39726121"},
                {"Other1_2", "-73.20288648"},
                {
                    "StoreNonCashCosts_2",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_2", "-3.6253603955135"},
                {"Onerousleasecredit_2", "0"},
                {"Depreciation1_2", "-65.04856321"},
                {"Amortisationofgoodwillandintangibleassets1_2", "0"},
                {"WriteoffofpropertyplantAndequipment_2", "0"},
                {"ABSAMerchantFee_2", "-61.6255206056757"},
                {"ClubProfit_2", "51.7620089643228"},
                {"SharedServices_2", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_2",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_2", "-13.8766537866594"},
                {"Depreciation2_2", "-0.0030309702516075"},
                {"Amortisationofgoodwillandintangibleassets_2", "0"},
                {"AssetDisposal1_2", "0"},
                {"Travel1_2", "-1.99419557647261"},
                {"Other2_2", "-0.971075702728968"},
                {
                    "Chainmanagementmerchandisse_2",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_2", "-52.4152877073373"},
                {"Travel2_2", "-3.48428328667802"},
                {"Depreciation3_2", "-0.694187746664991"},
                {"Other3_2", "-11.7323551122932"},
                {"CapexDepreciation1_2", "0"},
                {"AssetDisposal2_2", "0"},
                {"CorporateOverheads_2", "Chainadministrativecosts"},
                {"Chainadministrativecosts_2", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_2", "0"},
                {"Other_2", "-0.479036490707377"},
                {"Allocations_2", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_2",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_2", "-145.441714408881"},
                {"HeadOfficeRentalSavings_2", "0"},
                {"ITCosts_2", "-101.736156285846"},
                {"Cellularallocations_2", "0"},
                {"CreditAndFSallocations_2", "166.387257565834"},
                {"GrossProfit_2", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_2",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_2", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_2",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_2",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_2", "-36.9170233618892"},
                {"Corporateamortisationofgoodwillandintangibleassets2_2", "0"},
                {"Interest_2", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_2", "0"},
                {"Financinginterest_2", "-67.1964275731378"},
                {"EBIT_2", "GrossProfit + TotalExpenses"},
                {"EBITDA_2", "EBIT - DepreciationandAmmortisation"},
                {"EBT_2", "EBIT + Interest"},
                {"Tax_2", "34.4579796028313"},
                {"NetEarnings_2", "EBT - Tax"},
                {"PercentofRevenue_2", "GrossProfit / NetSales"},
                {"HyperionGrossSales_3", "2850.19065733"},
                {
                    "NetStores_3",
                    "CurrentNetStores_3 + NumberOfOpenedStores_3 - NumberOfClosedStores_3 - NumberOfSoldStores_3 + NumberOfStoresConvertedAcrossFormats_3"
                },
                {"EmployeeWageInflation_3", "EmployeeWageInflation_2 * (1 + EmployeeWageInflationFactor_3)"},
                {"RentalInflation_3", "RentalInflation_2 * (1 + RentalInflationFactor_3)"},
                {"StoreCapexInflation_3", "StoreCapexInflation_2 * (1 + StoreCapexInflationFactor_3)"},
                {"SalesGrowth_3", "1"},
                {
                    "ClosingCannibilisationEffect_3",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_3",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_3",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_3", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {"GrossSales_3", "2935.6963770499"},
                {"Markdowns_3", "-34.20228788796"},
                {"Promotions_3", "-51.30343183194"},
                {"LoyaltyDiscount_3", "-18.51949912"},
                {"CreditSales_3", "1365.03453451754"},
                {"CashSales_3", "NetSales - CreditSales"},
                {"TotalCostofSales_3", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_3", "RetekGrossProfit"},
                {"RetekGrossProfit_3", "1176.29460286"},
                {"IndirectCostofSales_3", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_3", "-53.7493636333149"},
                {"Rebates_3", "26.19640323"},
                {
                    "OtherAdjustments_3",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_3", "-0.00390137"},
                {"CentralStoreReturns_3", "-9.04752408"},
                {"SupplierAdjustments_3", "0"},
                {"OtherGrossProfitAdjustments_3", "0"},
                {"ConcessionFee_3", "15.86512847"},
                {"Advertising_3", "-0.0068055"},
                {
                    "TotalExpenses_3",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_3", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_3",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_3", "-242.61293638"},
                {"Stocklosses_3", "-33.86483075"},
                {"Rent_3", "-268.38402164"},
                {"Creditcardcommisions_3", "-20.68225134"},
                {"Security_3", "-23.96434481"},
                {"Cleaning_3", "-4.86947029"},
                {"Printingandstationery_3", "-10.73144145"},
                {"Communications_3", "-3.78686551"},
                {"WaterAndelectricity_3", "-46.85254982"},
                {"RepairsAndmaintenance_3", "-14.1202624"},
                {"Other1_3", "-77.54947481"},
                {
                    "StoreNonCashCosts_3",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_3", "-12.6463324470606"},
                {"Onerousleasecredit_3", "2.31770915"},
                {"Depreciation1_3", "-69.7637777"},
                {"Amortisationofgoodwillandintangibleassets1_3", "0"},
                {"WriteoffofpropertyplantAndequipment_3", "0"},
                {"ABSAMerchantFee_3", "-53.5075673819346"},
                {"ClubProfit_3", "51.8619244754807"},
                {"SharedServices_3", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_3",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_3", "-16.0040872183486"},
                {"Depreciation2_3", "-0.012870411413687"},
                {"Amortisationofgoodwillandintangibleassets_3", "0"},
                {"AssetDisposal1_3", "0"},
                {"Travel1_3", "-2.23757023960837"},
                {"Other2_3", "-1.3894038215536"},
                {
                    "Chainmanagementmerchandisse_3",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_3", "-43.5798325633593"},
                {"Travel2_3", "-2.07532556340075"},
                {"Depreciation3_3", "-0.939024233066053"},
                {"Other3_3", "-8.10533023467066"},
                {"CapexDepreciation1_3", "0"},
                {"AssetDisposal2_3", "-0.000808104408027455"},
                {"CorporateOverheads_3", "Chainadministrativecosts"},
                {"Chainadministrativecosts_3", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_3", "-9.62338789687033"},
                {"Other_3", "-0.184749973196826"},
                {"Allocations_3", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_3",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_3", "-133.262894863514"},
                {"HeadOfficeRentalSavings_3", "0"},
                {"ITCosts_3", "-96.27681937914"},
                {"Cellularallocations_3", "-3.94587973857981"},
                {"CreditAndFSallocations_3", "195.477404664368"},
                {"GrossProfit_3", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_3",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_3", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_3",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_3",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_3", "-30.5403699895582"},
                {"Corporateamortisationofgoodwillandintangibleassets2_3", "0"},
                {"Interest_3", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_3", "0"},
                {"Financinginterest_3", "-67.1964275731378"},
                {"EBIT_3", "GrossProfit + TotalExpenses"},
                {"EBITDA_3", "EBIT - DepreciationandAmmortisation"},
                {"EBT_3", "EBIT + Interest"},
                {"Tax_3", "40.1918822390602"},
                {"NetEarnings_3", "EBT - Tax"},
                {"PercentofRevenue_3", "GrossProfit / NetSales"},
                {"HyperionGrossSales_4", "2954.01444308"},
                {
                    "NetStores_4",
                    "CurrentNetStores_4 + NumberOfOpenedStores_4 - NumberOfClosedStores_4 - NumberOfSoldStores_4 + NumberOfStoresConvertedAcrossFormats_4"
                },
                {"EmployeeWageInflation_4", "EmployeeWageInflation_3 * (1 + EmployeeWageInflationFactor_4)"},
                {"RentalInflation_4", "RentalInflation_3 * (1 + RentalInflationFactor_4)"},
                {"StoreCapexInflation_4", "StoreCapexInflation_3 * (1 + StoreCapexInflationFactor_4)"},
                {"SalesGrowth_4", "1"},
                {
                    "ClosingCannibilisationEffect_4",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_4",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_4",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_4", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {"GrossSales_4", "3042.6348763724"},
                {"Markdowns_4", "-35.44817331696"},
                {"Promotions_4", "-53.17225997544"},
                {"LoyaltyDiscount_4", "-18.66215233"},
                {"CreditSales_4", "1208.51398593421"},
                {"CashSales_4", "NetSales - CreditSales"},
                {"TotalCostofSales_4", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_4", "RetekGrossProfit"},
                {"RetekGrossProfit_4", "1295.65551888"},
                {"IndirectCostofSales_4", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_4", "-50.9110454857759"},
                {"Rebates_4", "36.38445633"},
                {
                    "OtherAdjustments_4",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_4", "-0.00130289"},
                {"CentralStoreReturns_4", "-5.37894881"},
                {"SupplierAdjustments_4", "0"},
                {"OtherGrossProfitAdjustments_4", "0"},
                {"ConcessionFee_4", "6.39609246"},
                {"Advertising_4", "0"},
                {
                    "TotalExpenses_4",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_4", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_4",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_4", "-248.15498764"},
                {"Stocklosses_4", "-30.67324056"},
                {"Rent_4", "-286.48685675"},
                {"Creditcardcommisions_4", "-21.39430924"},
                {"Security_4", "-23.59428574"},
                {"Cleaning_4", "-5.19239921"},
                {"Printingandstationery_4", "-9.97232962"},
                {"Communications_4", "-3.74474797"},
                {"WaterAndelectricity_4", "-50.32787019"},
                {"RepairsAndmaintenance_4", "-14.89382396"},
                {"Other1_4", "-83.7462779"},
                {
                    "StoreNonCashCosts_4",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_4", "-9.24472337610207"},
                {"Onerousleasecredit_4", "1.473252"},
                {"Depreciation1_4", "-72.96436535"},
                {"Amortisationofgoodwillandintangibleassets1_4", "0"},
                {"WriteoffofpropertyplantAndequipment_4", "0"},
                {"ABSAMerchantFee_4", "-40.4397530598632"},
                {"ClubProfit_4", "56.6786331261543"},
                {"SharedServices_4", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_4",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_4", "-15.8804343210314"},
                {"Depreciation2_4", "-0.0136748759545062"},
                {"Amortisationofgoodwillandintangibleassets_4", "0"},
                {"AssetDisposal1_4", "0"},
                {"Travel1_4", "-2.28117257001371"},
                {"Other2_4", "-1.1867881581524"},
                {
                    "Chainmanagementmerchandisse_4",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_4", "-45.4365491860578"},
                {"Travel2_4", "-2.69954728695033"},
                {"Depreciation3_4", "-0.952267037009366"},
                {"Other3_4", "-9.13185083169433"},
                {"CapexDepreciation1_4", "0"},
                {"AssetDisposal2_4", "0"},
                {"CorporateOverheads_4", "Chainadministrativecosts"},
                {"Chainadministrativecosts_4", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_4", "-9.06911282874972"},
                {"Other_4", "-1.38992330220172"},
                {"Allocations_4", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_4",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_4", "-186.004667364827"},
                {"HeadOfficeRentalSavings_4", "0"},
                {"ITCosts_4", "-99.8019778855"},
                {"Cellularallocations_4", "-3.72676468507893"},
                {"CreditAndFSallocations_4", "156.720709695472"},
                {"GrossProfit_4", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_4",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_4", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_4",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_4",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_4", "-49.9987283019492"},
                {"Corporateamortisationofgoodwillandintangibleassets2_4", "0"},
                {"Interest_4", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_4", "0"},
                {"Financinginterest_4", "-67.1964275731378"},
                {"EBIT_4", "GrossProfit + TotalExpenses"},
                {"EBITDA_4", "EBIT - DepreciationandAmmortisation"},
                {"EBT_4", "EBIT + Interest"},
                {"Tax_4", "46.4273977640051"},
                {"NetEarnings_4", "EBT - Tax"},
                {"PercentofRevenue_4", "GrossProfit / NetSales"},
                {"HyperionGrossSales_5", "0"},
                {
                    "NetStores_5",
                    "CurrentNetStores_5 + NumberOfOpenedStores_5 - NumberOfClosedStores_5 - NumberOfSoldStores_5 + NumberOfStoresConvertedAcrossFormats_5"
                },
                {"EmployeeWageInflation_5", "EmployeeWageInflation_4 * (1 + EmployeeWageInflationFactor_5)"},
                {"RentalInflation_5", "RentalInflation_4 * (1 + RentalInflationFactor_5)"},
                {"StoreCapexInflation_5", "StoreCapexInflation_4 * (1 + StoreCapexInflationFactor_5)"},
                {"SalesGrowth_5", "(1 + SalesGrowthFactor_5)"},
                {
                    "ClosingCannibilisationEffect_5",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_5",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_5",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_5", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_5",
                    "(SalesGrowth_5) * (NetStores_5 * TradingDensity_5 * POW[1 + TradingDensityFactor_5, 1] * AverageStoreSize_5 / 1000000)"
                },
                {"Markdowns_5", "-1 * GrossSales_5 * MarkdownsPercentOfHyperionGrossSales_5 * MarkdownsFactor_5"},
                {"Promotions_5", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_5", "GrossSales_5 * LoyaltyDiscount_5 / GrossSales_5"},
                {"CreditSales_5", "NetSales_5 * CreditSales_5 / NetSales_5"},
                {"CashSales_5", "NetSales - CreditSales"},
                {"TotalCostofSales_5", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_5", "RetekGrossProfit"},
                {"RetekGrossProfit_5", "NetSales_5 * RetekGrossProfit_5 / NetSales_5"},
                {"IndirectCostofSales_5", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_5", "NetSales_5 * TCPSupplyChainCost_5 / NetSales_5"},
                {"Rebates_5", "NetSales_5 * Rebates_5 / NetSales_5"},
                {
                    "OtherAdjustments_5",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_5", "DirectCostofSales_5 * DiscountsandFGOs_5 / DirectCostofSales_5"},
                {"CentralStoreReturns_5", "DirectCostofSales_5 * CentralStoreReturns_5 / DirectCostofSales_5"},
                {"SupplierAdjustments_5", "DirectCostofSales_5 * SupplierAdjustments_5 / DirectCostofSales_5"},
                {"OtherGrossProfitAdjustments_5", "NetSales_5 * OtherGrossProfitAdjustments_5 / NetSales_5"},
                {"ConcessionFee_5", "NetSales_5 * ConcessionFee_5 / NetSales_5"},
                {"Advertising_5", "NetSales_5 * Advertising_5 / NetSales_5"},
                {
                    "TotalExpenses_5",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_5", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_5",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_5", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_5", "NetSales_5 * Stocklosses_5 / NetSales_5"},
                {"Rent_5", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_5", "CreditSales_5 * Creditcardcommisions_5 / CreditSales_5"},
                {"Security_5", "NetSales_5 * Security_5 / NetSales_5"},
                {"Cleaning_5", "NetSales_5 * Cleaning_5 / NetSales_5"},
                {"Printingandstationery_5", "NetSales_5 * Printingandstationery_5 / NetSales_5"},
                {"Communications_5", "NetSales_5 * Communications_5 / NetSales_5"},
                {"WaterAndelectricity_5", "Rent_5 * WaterAndelectricity_5 / Rent_5"},
                {"RepairsAndmaintenance_5", "Rent_5 * RepairsAndmaintenance_5 / Rent_5"},
                {"Other1_5", "NetSales_5 * Other1_5 / NetSales_5"},
                {
                    "StoreNonCashCosts_5",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_5", "Rent_5 * Operatingleaseadjustment_5 / Rent_5"},
                {"Onerousleasecredit_5", "Rent_5 * Onerousleasecredit_5 / Rent_5"},
                {"Depreciation1_5", "NetSales_5 * Depreciation1_5 / NetSales_5"},
                {"Amortisationofgoodwillandintangibleassets1_5", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_5",
                    "Depreciation1_5 * WriteoffofpropertyplantAndequipment_5 / Depreciation1_5"
                },
                {"ABSAMerchantFee_5", "CreditSales_5 * ABSAMerchantFee_5 / CreditSales_5"},
                {"ClubProfit_5", "NetSales_5 * ClubProfit_5 / NetSales_5"},
                {"SharedServices_5", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_5",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_5", "EmployeeCosts1_5 * EmployeeCosts2_5 / EmployeeCosts1_5"},
                {"Depreciation2_5", "Depreciation1_5 * Depreciation2_5 / Depreciation1_5"},
                {"Amortisationofgoodwillandintangibleassets_5", "0"},
                {"AssetDisposal1_5", "EmployeeCosts2_5 * AssetDisposal1_5 / EmployeeCosts2_5"},
                {"Travel1_5", "GrossProfit_5 * Travel1_5 / GrossProfit_5"},
                {"Other2_5", "NetSales_5 * Other2_5 / NetSales_5"},
                {
                    "Chainmanagementmerchandisse_5",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_5", "EmployeeCosts1_5 * EmployeeCosts3_5 / EmployeeCosts1_5"},
                {"Travel2_5", "GrossProfit_5 * Travel2_5 / GrossProfit_5"},
                {"Depreciation3_5", "Depreciation1_5 * Depreciation3_5 / Depreciation1_5"},
                {"Other3_5", "NetSales_5 * Other3_5 / NetSales_5"},
                {"CapexDepreciation1_5", "CapexDepreciation / 6"},
                {"AssetDisposal2_5", "Depreciation3_5 * AssetDisposal2_5 / Depreciation3_5"},
                {"CorporateOverheads_5", "Chainadministrativecosts"},
                {"Chainadministrativecosts_5", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_5", "EmployeeCosts1_5 * EmployeeCosts4_5 / EmployeeCosts1_5"},
                {"Other_5", "NetSales_5 * Other_5 / NetSales_5"},
                {"Allocations_5", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_5",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_5", "GrossProfit_5 * CorporateallocationsOther_5 / GrossProfit_5"},
                {"HeadOfficeRentalSavings_5", "0"},
                {"ITCosts_5", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_5", "GrossProfit_5 * Cellularallocations_5 / GrossProfit_5"},
                {"CreditAndFSallocations_5", "GrossProfit_5 * CreditAndFSallocations_5 / GrossProfit_5"},
                {"GrossProfit_5", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_5",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_5", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_5",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_5",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_5", "Depreciation1_5 * Corporatedepreciation_5 / Depreciation1_5"},
                {"Corporateamortisationofgoodwillandintangibleassets2_5", "0"},
                {"Interest_5", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_5", "0"},
                {"Financinginterest_5", "0"},
                {"EBIT_5", "GrossProfit + TotalExpenses"},
                {"EBITDA_5", "EBIT - DepreciationandAmmortisation"},
                {"EBT_5", "EBIT + Interest"},
                {"Tax_5", "CorporateTaxRate * EBT"},
                {"NetEarnings_5", "EBT - Tax"},
                {"PercentofRevenue_5", "GrossProfit / NetSales"},
                {"HyperionGrossSales_6", "0"},
                {
                    "NetStores_6",
                    "CurrentNetStores_6 + NumberOfOpenedStores_6 - NumberOfClosedStores_6 - NumberOfSoldStores_6 + NumberOfStoresConvertedAcrossFormats_6"
                },
                {"EmployeeWageInflation_6", "EmployeeWageInflation_5 * (1 + EmployeeWageInflationFactor_6)"},
                {"RentalInflation_6", "RentalInflation_5 * (1 + RentalInflationFactor_6)"},
                {"StoreCapexInflation_6", "StoreCapexInflation_5 * (1 + StoreCapexInflationFactor_6)"},
                {"SalesGrowth_6", "SalesGrowth_5 * (1 + SalesGrowthFactor_6)"},
                {
                    "ClosingCannibilisationEffect_6",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_6",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_6",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_6", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_6",
                    "(SalesGrowth_6) * (NetStores_6 * TradingDensity_6 * POW[1 + TradingDensityFactor_6, 1] * AverageStoreSize_6 / 1000000)"
                },
                {"Markdowns_6", "-1 * GrossSales_6 * MarkdownsPercentOfHyperionGrossSales_6 * MarkdownsFactor_6"},
                {"Promotions_6", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_6", "GrossSales_6 * LoyaltyDiscount_6 / GrossSales_6"},
                {"CreditSales_6", "NetSales_6 * CreditSales_6 / NetSales_6"},
                {"CashSales_6", "NetSales - CreditSales"},
                {"TotalCostofSales_6", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_6", "RetekGrossProfit"},
                {"RetekGrossProfit_6", "NetSales_6 * RetekGrossProfit_6 / NetSales_6"},
                {"IndirectCostofSales_6", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_6", "NetSales_6 * TCPSupplyChainCost_6 / NetSales_6"},
                {"Rebates_6", "NetSales_6 * Rebates_6 / NetSales_6"},
                {
                    "OtherAdjustments_6",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_6", "DirectCostofSales_6 * DiscountsandFGOs_6 / DirectCostofSales_6"},
                {"CentralStoreReturns_6", "DirectCostofSales_6 * CentralStoreReturns_6 / DirectCostofSales_6"},
                {"SupplierAdjustments_6", "DirectCostofSales_6 * SupplierAdjustments_6 / DirectCostofSales_6"},
                {"OtherGrossProfitAdjustments_6", "NetSales_6 * OtherGrossProfitAdjustments_6 / NetSales_6"},
                {"ConcessionFee_6", "NetSales_6 * ConcessionFee_6 / NetSales_6"},
                {"Advertising_6", "NetSales_6 * Advertising_6 / NetSales_6"},
                {
                    "TotalExpenses_6",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_6", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_6",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_6", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_6", "NetSales_6 * Stocklosses_6 / NetSales_6"},
                {"Rent_6", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_6", "CreditSales_6 * Creditcardcommisions_6 / CreditSales_6"},
                {"Security_6", "NetSales_6 * Security_6 / NetSales_6"},
                {"Cleaning_6", "NetSales_6 * Cleaning_6 / NetSales_6"},
                {"Printingandstationery_6", "NetSales_6 * Printingandstationery_6 / NetSales_6"},
                {"Communications_6", "NetSales_6 * Communications_6 / NetSales_6"},
                {"WaterAndelectricity_6", "Rent_6 * WaterAndelectricity_6 / Rent_6"},
                {"RepairsAndmaintenance_6", "Rent_6 * RepairsAndmaintenance_6 / Rent_6"},
                {"Other1_6", "NetSales_6 * Other1_6 / NetSales_6"},
                {
                    "StoreNonCashCosts_6",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_6", "Rent_6 * Operatingleaseadjustment_6 / Rent_6"},
                {"Onerousleasecredit_6", "Rent_6 * Onerousleasecredit_6 / Rent_6"},
                {"Depreciation1_6", "NetSales_6 * Depreciation1_6 / NetSales_6"},
                {"Amortisationofgoodwillandintangibleassets1_6", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_6",
                    "Depreciation1_6 * WriteoffofpropertyplantAndequipment_6 / Depreciation1_6"
                },
                {"ABSAMerchantFee_6", "CreditSales_6 * ABSAMerchantFee_6 / CreditSales_6"},
                {"ClubProfit_6", "NetSales_6 * ClubProfit_6 / NetSales_6"},
                {"SharedServices_6", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_6",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_6", "EmployeeCosts1_6 * EmployeeCosts2_6 / EmployeeCosts1_6"},
                {"Depreciation2_6", "Depreciation1_6 * Depreciation2_6 / Depreciation1_6"},
                {"Amortisationofgoodwillandintangibleassets_6", "0"},
                {"AssetDisposal1_6", "EmployeeCosts2_6 * AssetDisposal1_6 / EmployeeCosts2_6"},
                {"Travel1_6", "GrossProfit_6 * Travel1_6 / GrossProfit_6"},
                {"Other2_6", "NetSales_6 * Other2_6 / NetSales_6"},
                {
                    "Chainmanagementmerchandisse_6",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_6", "EmployeeCosts1_6 * EmployeeCosts3_6 / EmployeeCosts1_6"},
                {"Travel2_6", "GrossProfit_6 * Travel2_6 / GrossProfit_6"},
                {"Depreciation3_6", "Depreciation1_6 * Depreciation3_6 / Depreciation1_6"},
                {"Other3_6", "NetSales_6 * Other3_6 / NetSales_6"},
                {"CapexDepreciation1_6", "CapexDepreciation / 6"},
                {"AssetDisposal2_6", "Depreciation3_6 * AssetDisposal2_6 / Depreciation3_6"},
                {"CorporateOverheads_6", "Chainadministrativecosts"},
                {"Chainadministrativecosts_6", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_6", "EmployeeCosts1_6 * EmployeeCosts4_6 / EmployeeCosts1_6"},
                {"Other_6", "NetSales_6 * Other_6 / NetSales_6"},
                {"Allocations_6", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_6",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_6", "GrossProfit_6 * CorporateallocationsOther_6 / GrossProfit_6"},
                {"HeadOfficeRentalSavings_6", "0"},
                {"ITCosts_6", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_6", "GrossProfit_6 * Cellularallocations_6 / GrossProfit_6"},
                {"CreditAndFSallocations_6", "GrossProfit_6 * CreditAndFSallocations_6 / GrossProfit_6"},
                {"GrossProfit_6", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_6",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_6", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_6",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_6",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_6", "Depreciation1_6 * Corporatedepreciation_6 / Depreciation1_6"},
                {"Corporateamortisationofgoodwillandintangibleassets2_6", "0"},
                {"Interest_6", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_6", "0"},
                {"Financinginterest_6", "0"},
                {"EBIT_6", "GrossProfit + TotalExpenses"},
                {"EBITDA_6", "EBIT - DepreciationandAmmortisation"},
                {"EBT_6", "EBIT + Interest"},
                {"Tax_6", "CorporateTaxRate * EBT"},
                {"NetEarnings_6", "EBT - Tax"},
                {"PercentofRevenue_6", "GrossProfit / NetSales"},
                {"HyperionGrossSales_7", "0"},
                {
                    "NetStores_7",
                    "CurrentNetStores_7 + NumberOfOpenedStores_7 - NumberOfClosedStores_7 - NumberOfSoldStores_7 + NumberOfStoresConvertedAcrossFormats_7"
                },
                {"EmployeeWageInflation_7", "EmployeeWageInflation_6 * (1 + EmployeeWageInflationFactor_7)"},
                {"RentalInflation_7", "RentalInflation_6 * (1 + RentalInflationFactor_7)"},
                {"StoreCapexInflation_7", "StoreCapexInflation_6 * (1 + StoreCapexInflationFactor_7)"},
                {"SalesGrowth_7", "SalesGrowth_6 * (1 + SalesGrowthFactor_7)"},
                {
                    "ClosingCannibilisationEffect_7",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_7",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_7",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_7", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_7",
                    "(SalesGrowth_7) * (NetStores_7 * TradingDensity_7 * POW[1 + TradingDensityFactor_7, 1] * AverageStoreSize_7 / 1000000)"
                },
                {"Markdowns_7", "-1 * GrossSales_7 * MarkdownsPercentOfHyperionGrossSales_7 * MarkdownsFactor_7"},
                {"Promotions_7", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_7", "GrossSales_7 * LoyaltyDiscount_7 / GrossSales_7"},
                {"CreditSales_7", "NetSales_7 * CreditSales_7 / NetSales_7"},
                {"CashSales_7", "NetSales - CreditSales"},
                {"TotalCostofSales_7", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_7", "RetekGrossProfit"},
                {"RetekGrossProfit_7", "NetSales_7 * RetekGrossProfit_7 / NetSales_7"},
                {"IndirectCostofSales_7", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_7", "NetSales_7 * TCPSupplyChainCost_7 / NetSales_7"},
                {"Rebates_7", "NetSales_7 * Rebates_7 / NetSales_7"},
                {
                    "OtherAdjustments_7",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_7", "DirectCostofSales_7 * DiscountsandFGOs_7 / DirectCostofSales_7"},
                {"CentralStoreReturns_7", "DirectCostofSales_7 * CentralStoreReturns_7 / DirectCostofSales_7"},
                {"SupplierAdjustments_7", "DirectCostofSales_7 * SupplierAdjustments_7 / DirectCostofSales_7"},
                {"OtherGrossProfitAdjustments_7", "NetSales_7 * OtherGrossProfitAdjustments_7 / NetSales_7"},
                {"ConcessionFee_7", "NetSales_7 * ConcessionFee_7 / NetSales_7"},
                {"Advertising_7", "NetSales_7 * Advertising_7 / NetSales_7"},
                {
                    "TotalExpenses_7",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_7", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_7",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_7", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_7", "NetSales_7 * Stocklosses_7 / NetSales_7"},
                {"Rent_7", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_7", "CreditSales_7 * Creditcardcommisions_7 / CreditSales_7"},
                {"Security_7", "NetSales_7 * Security_7 / NetSales_7"},
                {"Cleaning_7", "NetSales_7 * Cleaning_7 / NetSales_7"},
                {"Printingandstationery_7", "NetSales_7 * Printingandstationery_7 / NetSales_7"},
                {"Communications_7", "NetSales_7 * Communications_7 / NetSales_7"},
                {"WaterAndelectricity_7", "Rent_7 * WaterAndelectricity_7 / Rent_7"},
                {"RepairsAndmaintenance_7", "Rent_7 * RepairsAndmaintenance_7 / Rent_7"},
                {"Other1_7", "NetSales_7 * Other1_7 / NetSales_7"},
                {
                    "StoreNonCashCosts_7",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_7", "Rent_7 * Operatingleaseadjustment_7 / Rent_7"},
                {"Onerousleasecredit_7", "Rent_7 * Onerousleasecredit_7 / Rent_7"},
                {"Depreciation1_7", "NetSales_7 * Depreciation1_7 / NetSales_7"},
                {"Amortisationofgoodwillandintangibleassets1_7", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_7",
                    "Depreciation1_7 * WriteoffofpropertyplantAndequipment_7 / Depreciation1_7"
                },
                {"ABSAMerchantFee_7", "CreditSales_7 * ABSAMerchantFee_7 / CreditSales_7"},
                {"ClubProfit_7", "NetSales_7 * ClubProfit_7 / NetSales_7"},
                {"SharedServices_7", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_7",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_7", "EmployeeCosts1_7 * EmployeeCosts2_7 / EmployeeCosts1_7"},
                {"Depreciation2_7", "Depreciation1_7 * Depreciation2_7 / Depreciation1_7"},
                {"Amortisationofgoodwillandintangibleassets_7", "0"},
                {"AssetDisposal1_7", "EmployeeCosts2_7 * AssetDisposal1_7 / EmployeeCosts2_7"},
                {"Travel1_7", "GrossProfit_7 * Travel1_7 / GrossProfit_7"},
                {"Other2_7", "NetSales_7 * Other2_7 / NetSales_7"},
                {
                    "Chainmanagementmerchandisse_7",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_7", "EmployeeCosts1_7 * EmployeeCosts3_7 / EmployeeCosts1_7"},
                {"Travel2_7", "GrossProfit_7 * Travel2_7 / GrossProfit_7"},
                {"Depreciation3_7", "Depreciation1_7 * Depreciation3_7 / Depreciation1_7"},
                {"Other3_7", "NetSales_7 * Other3_7 / NetSales_7"},
                {"CapexDepreciation1_7", "CapexDepreciation / 6"},
                {"AssetDisposal2_7", "Depreciation3_7 * AssetDisposal2_7 / Depreciation3_7"},
                {"CorporateOverheads_7", "Chainadministrativecosts"},
                {"Chainadministrativecosts_7", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_7", "EmployeeCosts1_7 * EmployeeCosts4_7 / EmployeeCosts1_7"},
                {"Other_7", "NetSales_7 * Other_7 / NetSales_7"},
                {"Allocations_7", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_7",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_7", "GrossProfit_7 * CorporateallocationsOther_7 / GrossProfit_7"},
                {"HeadOfficeRentalSavings_7", "0"},
                {"ITCosts_7", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_7", "GrossProfit_7 * Cellularallocations_7 / GrossProfit_7"},
                {"CreditAndFSallocations_7", "GrossProfit_7 * CreditAndFSallocations_7 / GrossProfit_7"},
                {"GrossProfit_7", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_7",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_7", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_7",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_7",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_7", "Depreciation1_7 * Corporatedepreciation_7 / Depreciation1_7"},
                {"Corporateamortisationofgoodwillandintangibleassets2_7", "0"},
                {"Interest_7", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_7", "0"},
                {"Financinginterest_7", "0"},
                {"EBIT_7", "GrossProfit + TotalExpenses"},
                {"EBITDA_7", "EBIT - DepreciationandAmmortisation"},
                {"EBT_7", "EBIT + Interest"},
                {"Tax_7", "CorporateTaxRate * EBT"},
                {"NetEarnings_7", "EBT - Tax"},
                {"PercentofRevenue_7", "GrossProfit / NetSales"},
                {"HyperionGrossSales_8", "0"},
                {
                    "NetStores_8",
                    "CurrentNetStores_8 + NumberOfOpenedStores_8 - NumberOfClosedStores_8 - NumberOfSoldStores_8 + NumberOfStoresConvertedAcrossFormats_8"
                },
                {"EmployeeWageInflation_8", "EmployeeWageInflation_7 * (1 + EmployeeWageInflationFactor_8)"},
                {"RentalInflation_8", "RentalInflation_7 * (1 + RentalInflationFactor_8)"},
                {"StoreCapexInflation_8", "StoreCapexInflation_7 * (1 + StoreCapexInflationFactor_8)"},
                {"SalesGrowth_8", "SalesGrowth_7 * (1 + SalesGrowthFactor_8)"},
                {
                    "ClosingCannibilisationEffect_8",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_8",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_8",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_8", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_8",
                    "(SalesGrowth_8) * (NetStores_8 * TradingDensity_8 * POW[1 + TradingDensityFactor_8, 1] * AverageStoreSize_8 / 1000000)"
                },
                {"Markdowns_8", "-1 * GrossSales_8 * MarkdownsPercentOfHyperionGrossSales_8 * MarkdownsFactor_8"},
                {"Promotions_8", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_8", "GrossSales_8 * LoyaltyDiscount_8 / GrossSales_8"},
                {"CreditSales_8", "NetSales_8 * CreditSales_8 / NetSales_8"},
                {"CashSales_8", "NetSales - CreditSales"},
                {"TotalCostofSales_8", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_8", "RetekGrossProfit"},
                {"RetekGrossProfit_8", "NetSales_8 * RetekGrossProfit_8 / NetSales_8"},
                {"IndirectCostofSales_8", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_8", "NetSales_8 * TCPSupplyChainCost_8 / NetSales_8"},
                {"Rebates_8", "NetSales_8 * Rebates_8 / NetSales_8"},
                {
                    "OtherAdjustments_8",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_8", "DirectCostofSales_8 * DiscountsandFGOs_8 / DirectCostofSales_8"},
                {"CentralStoreReturns_8", "DirectCostofSales_8 * CentralStoreReturns_8 / DirectCostofSales_8"},
                {"SupplierAdjustments_8", "DirectCostofSales_8 * SupplierAdjustments_8 / DirectCostofSales_8"},
                {"OtherGrossProfitAdjustments_8", "NetSales_8 * OtherGrossProfitAdjustments_8 / NetSales_8"},
                {"ConcessionFee_8", "NetSales_8 * ConcessionFee_8 / NetSales_8"},
                {"Advertising_8", "NetSales_8 * Advertising_8 / NetSales_8"},
                {
                    "TotalExpenses_8",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_8", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_8",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_8", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_8", "NetSales_8 * Stocklosses_8 / NetSales_8"},
                {"Rent_8", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_8", "CreditSales_8 * Creditcardcommisions_8 / CreditSales_8"},
                {"Security_8", "NetSales_8 * Security_8 / NetSales_8"},
                {"Cleaning_8", "NetSales_8 * Cleaning_8 / NetSales_8"},
                {"Printingandstationery_8", "NetSales_8 * Printingandstationery_8 / NetSales_8"},
                {"Communications_8", "NetSales_8 * Communications_8 / NetSales_8"},
                {"WaterAndelectricity_8", "Rent_8 * WaterAndelectricity_8 / Rent_8"},
                {"RepairsAndmaintenance_8", "Rent_8 * RepairsAndmaintenance_8 / Rent_8"},
                {"Other1_8", "NetSales_8 * Other1_8 / NetSales_8"},
                {
                    "StoreNonCashCosts_8",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_8", "Rent_8 * Operatingleaseadjustment_8 / Rent_8"},
                {"Onerousleasecredit_8", "Rent_8 * Onerousleasecredit_8 / Rent_8"},
                {"Depreciation1_8", "NetSales_8 * Depreciation1_8 / NetSales_8"},
                {"Amortisationofgoodwillandintangibleassets1_8", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_8",
                    "Depreciation1_8 * WriteoffofpropertyplantAndequipment_8 / Depreciation1_8"
                },
                {"ABSAMerchantFee_8", "CreditSales_8 * ABSAMerchantFee_8 / CreditSales_8"},
                {"ClubProfit_8", "NetSales_8 * ClubProfit_8 / NetSales_8"},
                {"SharedServices_8", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_8",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_8", "EmployeeCosts1_8 * EmployeeCosts2_8 / EmployeeCosts1_8"},
                {"Depreciation2_8", "Depreciation1_8 * Depreciation2_8 / Depreciation1_8"},
                {"Amortisationofgoodwillandintangibleassets_8", "0"},
                {"AssetDisposal1_8", "EmployeeCosts2_8 * AssetDisposal1_8 / EmployeeCosts2_8"},
                {"Travel1_8", "GrossProfit_8 * Travel1_8 / GrossProfit_8"},
                {"Other2_8", "NetSales_8 * Other2_8 / NetSales_8"},
                {
                    "Chainmanagementmerchandisse_8",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_8", "EmployeeCosts1_8 * EmployeeCosts3_8 / EmployeeCosts1_8"},
                {"Travel2_8", "GrossProfit_8 * Travel2_8 / GrossProfit_8"},
                {"Depreciation3_8", "Depreciation1_8 * Depreciation3_8 / Depreciation1_8"},
                {"Other3_8", "NetSales_8 * Other3_8 / NetSales_8"},
                {"CapexDepreciation1_8", "CapexDepreciation / 6"},
                {"AssetDisposal2_8", "Depreciation3_8 * AssetDisposal2_8 / Depreciation3_8"},
                {"CorporateOverheads_8", "Chainadministrativecosts"},
                {"Chainadministrativecosts_8", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_8", "EmployeeCosts1_8 * EmployeeCosts4_8 / EmployeeCosts1_8"},
                {"Other_8", "NetSales_8 * Other_8 / NetSales_8"},
                {"Allocations_8", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_8",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_8", "GrossProfit_8 * CorporateallocationsOther_8 / GrossProfit_8"},
                {"HeadOfficeRentalSavings_8", "0"},
                {"ITCosts_8", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_8", "GrossProfit_8 * Cellularallocations_8 / GrossProfit_8"},
                {"CreditAndFSallocations_8", "GrossProfit_8 * CreditAndFSallocations_8 / GrossProfit_8"},
                {"GrossProfit_8", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_8",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_8", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_8",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_8",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_8", "Depreciation1_8 * Corporatedepreciation_8 / Depreciation1_8"},
                {"Corporateamortisationofgoodwillandintangibleassets2_8", "0"},
                {"Interest_8", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_8", "0"},
                {"Financinginterest_8", "0"},
                {"EBIT_8", "GrossProfit + TotalExpenses"},
                {"EBITDA_8", "EBIT - DepreciationandAmmortisation"},
                {"EBT_8", "EBIT + Interest"},
                {"Tax_8", "CorporateTaxRate * EBT"},
                {"NetEarnings_8", "EBT - Tax"},
                {"PercentofRevenue_8", "GrossProfit / NetSales"},
                {"HyperionGrossSales_9", "0"},
                {
                    "NetStores_9",
                    "CurrentNetStores_9 + NumberOfOpenedStores_9 - NumberOfClosedStores_9 - NumberOfSoldStores_9 + NumberOfStoresConvertedAcrossFormats_9"
                },
                {"EmployeeWageInflation_9", "EmployeeWageInflation_8 * (1 + EmployeeWageInflationFactor_9)"},
                {"RentalInflation_9", "RentalInflation_8 * (1 + RentalInflationFactor_9)"},
                {"StoreCapexInflation_9", "StoreCapexInflation_8 * (1 + StoreCapexInflationFactor_9)"},
                {"SalesGrowth_9", "SalesGrowth_8 * (1 + SalesGrowthFactor_9)"},
                {
                    "ClosingCannibilisationEffect_9",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_9",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_9",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_9", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_9",
                    "(SalesGrowth_9) * (NetStores_9 * TradingDensity_9 * POW[1 + TradingDensityFactor_9, 1] * AverageStoreSize_9 / 1000000)"
                },
                {"Markdowns_9", "-1 * GrossSales_9 * MarkdownsPercentOfHyperionGrossSales_9 * MarkdownsFactor_9"},
                {"Promotions_9", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_9", "GrossSales_9 * LoyaltyDiscount_9 / GrossSales_9"},
                {"CreditSales_9", "NetSales_9 * CreditSales_9 / NetSales_9"},
                {"CashSales_9", "NetSales - CreditSales"},
                {"TotalCostofSales_9", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_9", "RetekGrossProfit"},
                {"RetekGrossProfit_9", "NetSales_9 * RetekGrossProfit_9 / NetSales_9"},
                {"IndirectCostofSales_9", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_9", "NetSales_9 * TCPSupplyChainCost_9 / NetSales_9"},
                {"Rebates_9", "NetSales_9 * Rebates_9 / NetSales_9"},
                {
                    "OtherAdjustments_9",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_9", "DirectCostofSales_9 * DiscountsandFGOs_9 / DirectCostofSales_9"},
                {"CentralStoreReturns_9", "DirectCostofSales_9 * CentralStoreReturns_9 / DirectCostofSales_9"},
                {"SupplierAdjustments_9", "DirectCostofSales_9 * SupplierAdjustments_9 / DirectCostofSales_9"},
                {"OtherGrossProfitAdjustments_9", "NetSales_9 * OtherGrossProfitAdjustments_9 / NetSales_9"},
                {"ConcessionFee_9", "NetSales_9 * ConcessionFee_9 / NetSales_9"},
                {"Advertising_9", "NetSales_9 * Advertising_9 / NetSales_9"},
                {
                    "TotalExpenses_9",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_9", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_9",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_9", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_9", "NetSales_9 * Stocklosses_9 / NetSales_9"},
                {"Rent_9", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_9", "CreditSales_9 * Creditcardcommisions_9 / CreditSales_9"},
                {"Security_9", "NetSales_9 * Security_9 / NetSales_9"},
                {"Cleaning_9", "NetSales_9 * Cleaning_9 / NetSales_9"},
                {"Printingandstationery_9", "NetSales_9 * Printingandstationery_9 / NetSales_9"},
                {"Communications_9", "NetSales_9 * Communications_9 / NetSales_9"},
                {"WaterAndelectricity_9", "Rent_9 * WaterAndelectricity_9 / Rent_9"},
                {"RepairsAndmaintenance_9", "Rent_9 * RepairsAndmaintenance_9 / Rent_9"},
                {"Other1_9", "NetSales_9 * Other1_9 / NetSales_9"},
                {
                    "StoreNonCashCosts_9",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_9", "Rent_9 * Operatingleaseadjustment_9 / Rent_9"},
                {"Onerousleasecredit_9", "Rent_9 * Onerousleasecredit_9 / Rent_9"},
                {"Depreciation1_9", "NetSales_9 * Depreciation1_9 / NetSales_9"},
                {"Amortisationofgoodwillandintangibleassets1_9", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_9",
                    "Depreciation1_9 * WriteoffofpropertyplantAndequipment_9 / Depreciation1_9"
                },
                {"ABSAMerchantFee_9", "CreditSales_9 * ABSAMerchantFee_9 / CreditSales_9"},
                {"ClubProfit_9", "NetSales_9 * ClubProfit_9 / NetSales_9"},
                {"SharedServices_9", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_9",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_9", "EmployeeCosts1_9 * EmployeeCosts2_9 / EmployeeCosts1_9"},
                {"Depreciation2_9", "Depreciation1_9 * Depreciation2_9 / Depreciation1_9"},
                {"Amortisationofgoodwillandintangibleassets_9", "0"},
                {"AssetDisposal1_9", "EmployeeCosts2_9 * AssetDisposal1_9 / EmployeeCosts2_9"},
                {"Travel1_9", "GrossProfit_9 * Travel1_9 / GrossProfit_9"},
                {"Other2_9", "NetSales_9 * Other2_9 / NetSales_9"},
                {
                    "Chainmanagementmerchandisse_9",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_9", "EmployeeCosts1_9 * EmployeeCosts3_9 / EmployeeCosts1_9"},
                {"Travel2_9", "GrossProfit_9 * Travel2_9 / GrossProfit_9"},
                {"Depreciation3_9", "Depreciation1_9 * Depreciation3_9 / Depreciation1_9"},
                {"Other3_9", "NetSales_9 * Other3_9 / NetSales_9"},
                {"CapexDepreciation1_9", "CapexDepreciation / 6"},
                {"AssetDisposal2_9", "Depreciation3_9 * AssetDisposal2_9 / Depreciation3_9"},
                {"CorporateOverheads_9", "Chainadministrativecosts"},
                {"Chainadministrativecosts_9", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_9", "EmployeeCosts1_9 * EmployeeCosts4_9 / EmployeeCosts1_9"},
                {"Other_9", "NetSales_9 * Other_9 / NetSales_9"},
                {"Allocations_9", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_9",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_9", "GrossProfit_9 * CorporateallocationsOther_9 / GrossProfit_9"},
                {"HeadOfficeRentalSavings_9", "0"},
                {"ITCosts_9", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_9", "GrossProfit_9 * Cellularallocations_9 / GrossProfit_9"},
                {"CreditAndFSallocations_9", "GrossProfit_9 * CreditAndFSallocations_9 / GrossProfit_9"},
                {"GrossProfit_9", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_9",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_9", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_9",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_9",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_9", "Depreciation1_9 * Corporatedepreciation_9 / Depreciation1_9"},
                {"Corporateamortisationofgoodwillandintangibleassets2_9", "0"},
                {"Interest_9", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_9", "0"},
                {"Financinginterest_9", "0"},
                {"EBIT_9", "GrossProfit + TotalExpenses"},
                {"EBITDA_9", "EBIT - DepreciationandAmmortisation"},
                {"EBT_9", "EBIT + Interest"},
                {"Tax_9", "CorporateTaxRate * EBT"},
                {"NetEarnings_9", "EBT - Tax"},
                {"PercentofRevenue_9", "GrossProfit / NetSales"},
                {"HyperionGrossSales_10", "0"},
                {
                    "NetStores_10",
                    "CurrentNetStores_10 + NumberOfOpenedStores_10 - NumberOfClosedStores_10 - NumberOfSoldStores_10 + NumberOfStoresConvertedAcrossFormats_10"
                },
                {"EmployeeWageInflation_10", "EmployeeWageInflation_9 * (1 + EmployeeWageInflationFactor_10)"},
                {"RentalInflation_10", "RentalInflation_9 * (1 + RentalInflationFactor_10)"},
                {"StoreCapexInflation_10", "StoreCapexInflation_9 * (1 + StoreCapexInflationFactor_10)"},
                {"SalesGrowth_10", "SalesGrowth_9 * (1 + SalesGrowthFactor_10)"},
                {
                    "ClosingCannibilisationEffect_10",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_10",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_10",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_10", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_10",
                    "(SalesGrowth_10) * (NetStores_10 * TradingDensity_10 * POW[1 + TradingDensityFactor_10, 1] * AverageStoreSize_10 / 1000000)"
                },
                {"Markdowns_10", "-1 * GrossSales_10 * MarkdownsPercentOfHyperionGrossSales_10 * MarkdownsFactor_10"},
                {"Promotions_10", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_10", "GrossSales_10 * LoyaltyDiscount_10 / GrossSales_10"},
                {"CreditSales_10", "NetSales_10 * CreditSales_10 / NetSales_10"},
                {"CashSales_10", "NetSales - CreditSales"},
                {"TotalCostofSales_10", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_10", "RetekGrossProfit"},
                {"RetekGrossProfit_10", "NetSales_10 * RetekGrossProfit_10 / NetSales_10"},
                {"IndirectCostofSales_10", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_10", "NetSales_10 * TCPSupplyChainCost_10 / NetSales_10"},
                {"Rebates_10", "NetSales_10 * Rebates_10 / NetSales_10"},
                {
                    "OtherAdjustments_10",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_10", "DirectCostofSales_10 * DiscountsandFGOs_10 / DirectCostofSales_10"},
                {"CentralStoreReturns_10", "DirectCostofSales_10 * CentralStoreReturns_10 / DirectCostofSales_10"},
                {"SupplierAdjustments_10", "DirectCostofSales_10 * SupplierAdjustments_10 / DirectCostofSales_10"},
                {"OtherGrossProfitAdjustments_10", "NetSales_10 * OtherGrossProfitAdjustments_10 / NetSales_10"},
                {"ConcessionFee_10", "NetSales_10 * ConcessionFee_10 / NetSales_10"},
                {"Advertising_10", "NetSales_10 * Advertising_10 / NetSales_10"},
                {
                    "TotalExpenses_10",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_10", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_10",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_10", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_10", "NetSales_10 * Stocklosses_10 / NetSales_10"},
                {"Rent_10", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_10", "CreditSales_10 * Creditcardcommisions_10 / CreditSales_10"},
                {"Security_10", "NetSales_10 * Security_10 / NetSales_10"},
                {"Cleaning_10", "NetSales_10 * Cleaning_10 / NetSales_10"},
                {"Printingandstationery_10", "NetSales_10 * Printingandstationery_10 / NetSales_10"},
                {"Communications_10", "NetSales_10 * Communications_10 / NetSales_10"},
                {"WaterAndelectricity_10", "Rent_10 * WaterAndelectricity_10 / Rent_10"},
                {"RepairsAndmaintenance_10", "Rent_10 * RepairsAndmaintenance_10 / Rent_10"},
                {"Other1_10", "NetSales_10 * Other1_10 / NetSales_10"},
                {
                    "StoreNonCashCosts_10",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_10", "Rent_10 * Operatingleaseadjustment_10 / Rent_10"},
                {"Onerousleasecredit_10", "Rent_10 * Onerousleasecredit_10 / Rent_10"},
                {"Depreciation1_10", "NetSales_10 * Depreciation1_10 / NetSales_10"},
                {"Amortisationofgoodwillandintangibleassets1_10", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_10",
                    "Depreciation1_10 * WriteoffofpropertyplantAndequipment_10 / Depreciation1_10"
                },
                {"ABSAMerchantFee_10", "CreditSales_10 * ABSAMerchantFee_10 / CreditSales_10"},
                {"ClubProfit_10", "NetSales_10 * ClubProfit_10 / NetSales_10"},
                {"SharedServices_10", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_10",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_10", "EmployeeCosts1_10 * EmployeeCosts2_10 / EmployeeCosts1_10"},
                {"Depreciation2_10", "Depreciation1_10 * Depreciation2_10 / Depreciation1_10"},
                {"Amortisationofgoodwillandintangibleassets_10", "0"},
                {"AssetDisposal1_10", "EmployeeCosts2_10 * AssetDisposal1_10 / EmployeeCosts2_10"},
                {"Travel1_10", "GrossProfit_10 * Travel1_10 / GrossProfit_10"},
                {"Other2_10", "NetSales_10 * Other2_10 / NetSales_10"},
                {
                    "Chainmanagementmerchandisse_10",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_10", "EmployeeCosts1_10 * EmployeeCosts3_10 / EmployeeCosts1_10"},
                {"Travel2_10", "GrossProfit_10 * Travel2_10 / GrossProfit_10"},
                {"Depreciation3_10", "Depreciation1_10 * Depreciation3_10 / Depreciation1_10"},
                {"Other3_10", "NetSales_10 * Other3_10 / NetSales_10"},
                {"CapexDepreciation1_10", "CapexDepreciation / 6"},
                {"AssetDisposal2_10", "Depreciation3_10 * AssetDisposal2_10 / Depreciation3_10"},
                {"CorporateOverheads_10", "Chainadministrativecosts"},
                {"Chainadministrativecosts_10", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_10", "EmployeeCosts1_10 * EmployeeCosts4_10 / EmployeeCosts1_10"},
                {"Other_10", "NetSales_10 * Other_10 / NetSales_10"},
                {"Allocations_10", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_10",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_10", "GrossProfit_10 * CorporateallocationsOther_10 / GrossProfit_10"},
                {"HeadOfficeRentalSavings_10", "0"},
                {"ITCosts_10", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_10", "GrossProfit_10 * Cellularallocations_10 / GrossProfit_10"},
                {"CreditAndFSallocations_10", "GrossProfit_10 * CreditAndFSallocations_10 / GrossProfit_10"},
                {"GrossProfit_10", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_10",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_10", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_10",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_10",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_10", "Depreciation1_10 * Corporatedepreciation_10 / Depreciation1_10"},
                {"Corporateamortisationofgoodwillandintangibleassets2_10", "0"},
                {"Interest_10", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_10", "0"},
                {"Financinginterest_10", "0"},
                {"EBIT_10", "GrossProfit + TotalExpenses"},
                {"EBITDA_10", "EBIT - DepreciationandAmmortisation"},
                {"EBT_10", "EBIT + Interest"},
                {"Tax_10", "CorporateTaxRate * EBT"},
                {"NetEarnings_10", "EBT - Tax"},
                {"PercentofRevenue_10", "GrossProfit / NetSales"},
                {"HyperionGrossSales_11", "0"},
                {
                    "NetStores_11",
                    "CurrentNetStores_11 + NumberOfOpenedStores_11 - NumberOfClosedStores_11 - NumberOfSoldStores_11 + NumberOfStoresConvertedAcrossFormats_11"
                },
                {"EmployeeWageInflation_11", "EmployeeWageInflation_10 * (1 + EmployeeWageInflationFactor_11)"},
                {"RentalInflation_11", "RentalInflation_10 * (1 + RentalInflationFactor_11)"},
                {"StoreCapexInflation_11", "StoreCapexInflation_10 * (1 + StoreCapexInflationFactor_11)"},
                {"SalesGrowth_11", "SalesGrowth_10 * (1 + SalesGrowthFactor_11)"},
                {
                    "ClosingCannibilisationEffect_11",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_11",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_11",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_11", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_11",
                    "(SalesGrowth_11) * (NetStores_11 * TradingDensity_11 * POW[1 + TradingDensityFactor_11, 1] * AverageStoreSize_11 / 1000000)"
                },
                {"Markdowns_11", "-1 * GrossSales_11 * MarkdownsPercentOfHyperionGrossSales_11 * MarkdownsFactor_11"},
                {"Promotions_11", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_11", "GrossSales_11 * LoyaltyDiscount_11 / GrossSales_11"},
                {"CreditSales_11", "NetSales_11 * CreditSales_11 / NetSales_11"},
                {"CashSales_11", "NetSales - CreditSales"},
                {"TotalCostofSales_11", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_11", "RetekGrossProfit"},
                {"RetekGrossProfit_11", "NetSales_11 * RetekGrossProfit_11 / NetSales_11"},
                {"IndirectCostofSales_11", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_11", "NetSales_11 * TCPSupplyChainCost_11 / NetSales_11"},
                {"Rebates_11", "NetSales_11 * Rebates_11 / NetSales_11"},
                {
                    "OtherAdjustments_11",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_11", "DirectCostofSales_11 * DiscountsandFGOs_11 / DirectCostofSales_11"},
                {"CentralStoreReturns_11", "DirectCostofSales_11 * CentralStoreReturns_11 / DirectCostofSales_11"},
                {"SupplierAdjustments_11", "DirectCostofSales_11 * SupplierAdjustments_11 / DirectCostofSales_11"},
                {"OtherGrossProfitAdjustments_11", "NetSales_11 * OtherGrossProfitAdjustments_11 / NetSales_11"},
                {"ConcessionFee_11", "NetSales_11 * ConcessionFee_11 / NetSales_11"},
                {"Advertising_11", "NetSales_11 * Advertising_11 / NetSales_11"},
                {
                    "TotalExpenses_11",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_11", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_11",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_11", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_11", "NetSales_11 * Stocklosses_11 / NetSales_11"},
                {"Rent_11", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_11", "CreditSales_11 * Creditcardcommisions_11 / CreditSales_11"},
                {"Security_11", "NetSales_11 * Security_11 / NetSales_11"},
                {"Cleaning_11", "NetSales_11 * Cleaning_11 / NetSales_11"},
                {"Printingandstationery_11", "NetSales_11 * Printingandstationery_11 / NetSales_11"},
                {"Communications_11", "NetSales_11 * Communications_11 / NetSales_11"},
                {"WaterAndelectricity_11", "Rent_11 * WaterAndelectricity_11 / Rent_11"},
                {"RepairsAndmaintenance_11", "Rent_11 * RepairsAndmaintenance_11 / Rent_11"},
                {"Other1_11", "NetSales_11 * Other1_11 / NetSales_11"},
                {
                    "StoreNonCashCosts_11",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_11", "Rent_11 * Operatingleaseadjustment_11 / Rent_11"},
                {"Onerousleasecredit_11", "Rent_11 * Onerousleasecredit_11 / Rent_11"},
                {"Depreciation1_11", "NetSales_11 * Depreciation1_11 / NetSales_11"},
                {"Amortisationofgoodwillandintangibleassets1_11", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_11",
                    "Depreciation1_11 * WriteoffofpropertyplantAndequipment_11 / Depreciation1_11"
                },
                {"ABSAMerchantFee_11", "CreditSales_11 * ABSAMerchantFee_11 / CreditSales_11"},
                {"ClubProfit_11", "NetSales_11 * ClubProfit_11 / NetSales_11"},
                {"SharedServices_11", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_11",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_11", "EmployeeCosts1_11 * EmployeeCosts2_11 / EmployeeCosts1_11"},
                {"Depreciation2_11", "Depreciation1_11 * Depreciation2_11 / Depreciation1_11"},
                {"Amortisationofgoodwillandintangibleassets_11", "0"},
                {"AssetDisposal1_11", "EmployeeCosts2_11 * AssetDisposal1_11 / EmployeeCosts2_11"},
                {"Travel1_11", "GrossProfit_11 * Travel1_11 / GrossProfit_11"},
                {"Other2_11", "NetSales_11 * Other2_11 / NetSales_11"},
                {
                    "Chainmanagementmerchandisse_11",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_11", "EmployeeCosts1_11 * EmployeeCosts3_11 / EmployeeCosts1_11"},
                {"Travel2_11", "GrossProfit_11 * Travel2_11 / GrossProfit_11"},
                {"Depreciation3_11", "Depreciation1_11 * Depreciation3_11 / Depreciation1_11"},
                {"Other3_11", "NetSales_11 * Other3_11 / NetSales_11"},
                {"CapexDepreciation1_11", "CapexDepreciation / 6"},
                {"AssetDisposal2_11", "Depreciation3_11 * AssetDisposal2_11 / Depreciation3_11"},
                {"CorporateOverheads_11", "Chainadministrativecosts"},
                {"Chainadministrativecosts_11", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_11", "EmployeeCosts1_11 * EmployeeCosts4_11 / EmployeeCosts1_11"},
                {"Other_11", "NetSales_11 * Other_11 / NetSales_11"},
                {"Allocations_11", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_11",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_11", "GrossProfit_11 * CorporateallocationsOther_11 / GrossProfit_11"},
                {"HeadOfficeRentalSavings_11", "0"},
                {"ITCosts_11", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_11", "GrossProfit_11 * Cellularallocations_11 / GrossProfit_11"},
                {"CreditAndFSallocations_11", "GrossProfit_11 * CreditAndFSallocations_11 / GrossProfit_11"},
                {"GrossProfit_11", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_11",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_11", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_11",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_11",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_11", "Depreciation1_11 * Corporatedepreciation_11 / Depreciation1_11"},
                {"Corporateamortisationofgoodwillandintangibleassets2_11", "0"},
                {"Interest_11", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_11", "0"},
                {"Financinginterest_11", "0"},
                {"EBIT_11", "GrossProfit + TotalExpenses"},
                {"EBITDA_11", "EBIT - DepreciationandAmmortisation"},
                {"EBT_11", "EBIT + Interest"},
                {"Tax_11", "CorporateTaxRate * EBT"},
                {"NetEarnings_11", "EBT - Tax"},
                {"PercentofRevenue_11", "GrossProfit / NetSales"},
                {"HyperionGrossSales_12", "0"},
                {
                    "NetStores_12",
                    "CurrentNetStores_12 + NumberOfOpenedStores_12 - NumberOfClosedStores_12 - NumberOfSoldStores_12 + NumberOfStoresConvertedAcrossFormats_12"
                },
                {"EmployeeWageInflation_12", "EmployeeWageInflation_11 * (1 + EmployeeWageInflationFactor_12)"},
                {"RentalInflation_12", "RentalInflation_11 * (1 + RentalInflationFactor_12)"},
                {"StoreCapexInflation_12", "StoreCapexInflation_11 * (1 + StoreCapexInflationFactor_12)"},
                {"SalesGrowth_12", "SalesGrowth_11 * (1 + SalesGrowthFactor_12)"},
                {
                    "ClosingCannibilisationEffect_12",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_12",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_12",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_12", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_12",
                    "(SalesGrowth_12) * (NetStores_12 * TradingDensity_12 * POW[1 + TradingDensityFactor_12, 1] * AverageStoreSize_12 / 1000000)"
                },
                {"Markdowns_12", "-1 * GrossSales_12 * MarkdownsPercentOfHyperionGrossSales_12 * MarkdownsFactor_12"},
                {"Promotions_12", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_12", "GrossSales_12 * LoyaltyDiscount_12 / GrossSales_12"},
                {"CreditSales_12", "NetSales_12 * CreditSales_12 / NetSales_12"},
                {"CashSales_12", "NetSales - CreditSales"},
                {"TotalCostofSales_12", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_12", "RetekGrossProfit"},
                {"RetekGrossProfit_12", "NetSales_12 * RetekGrossProfit_12 / NetSales_12"},
                {"IndirectCostofSales_12", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_12", "NetSales_12 * TCPSupplyChainCost_12 / NetSales_12"},
                {"Rebates_12", "NetSales_12 * Rebates_12 / NetSales_12"},
                {
                    "OtherAdjustments_12",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_12", "DirectCostofSales_12 * DiscountsandFGOs_12 / DirectCostofSales_12"},
                {"CentralStoreReturns_12", "DirectCostofSales_12 * CentralStoreReturns_12 / DirectCostofSales_12"},
                {"SupplierAdjustments_12", "DirectCostofSales_12 * SupplierAdjustments_12 / DirectCostofSales_12"},
                {"OtherGrossProfitAdjustments_12", "NetSales_12 * OtherGrossProfitAdjustments_12 / NetSales_12"},
                {"ConcessionFee_12", "NetSales_12 * ConcessionFee_12 / NetSales_12"},
                {"Advertising_12", "NetSales_12 * Advertising_12 / NetSales_12"},
                {
                    "TotalExpenses_12",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_12", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_12",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_12", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_12", "NetSales_12 * Stocklosses_12 / NetSales_12"},
                {"Rent_12", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_12", "CreditSales_12 * Creditcardcommisions_12 / CreditSales_12"},
                {"Security_12", "NetSales_12 * Security_12 / NetSales_12"},
                {"Cleaning_12", "NetSales_12 * Cleaning_12 / NetSales_12"},
                {"Printingandstationery_12", "NetSales_12 * Printingandstationery_12 / NetSales_12"},
                {"Communications_12", "NetSales_12 * Communications_12 / NetSales_12"},
                {"WaterAndelectricity_12", "Rent_12 * WaterAndelectricity_12 / Rent_12"},
                {"RepairsAndmaintenance_12", "Rent_12 * RepairsAndmaintenance_12 / Rent_12"},
                {"Other1_12", "NetSales_12 * Other1_12 / NetSales_12"},
                {
                    "StoreNonCashCosts_12",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_12", "Rent_12 * Operatingleaseadjustment_12 / Rent_12"},
                {"Onerousleasecredit_12", "Rent_12 * Onerousleasecredit_12 / Rent_12"},
                {"Depreciation1_12", "NetSales_12 * Depreciation1_12 / NetSales_12"},
                {"Amortisationofgoodwillandintangibleassets1_12", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_12",
                    "Depreciation1_12 * WriteoffofpropertyplantAndequipment_12 / Depreciation1_12"
                },
                {"ABSAMerchantFee_12", "CreditSales_12 * ABSAMerchantFee_12 / CreditSales_12"},
                {"ClubProfit_12", "NetSales_12 * ClubProfit_12 / NetSales_12"},
                {"SharedServices_12", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_12",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_12", "EmployeeCosts1_12 * EmployeeCosts2_12 / EmployeeCosts1_12"},
                {"Depreciation2_12", "Depreciation1_12 * Depreciation2_12 / Depreciation1_12"},
                {"Amortisationofgoodwillandintangibleassets_12", "0"},
                {"AssetDisposal1_12", "EmployeeCosts2_12 * AssetDisposal1_12 / EmployeeCosts2_12"},
                {"Travel1_12", "GrossProfit_12 * Travel1_12 / GrossProfit_12"},
                {"Other2_12", "NetSales_12 * Other2_12 / NetSales_12"},
                {
                    "Chainmanagementmerchandisse_12",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_12", "EmployeeCosts1_12 * EmployeeCosts3_12 / EmployeeCosts1_12"},
                {"Travel2_12", "GrossProfit_12 * Travel2_12 / GrossProfit_12"},
                {"Depreciation3_12", "Depreciation1_12 * Depreciation3_12 / Depreciation1_12"},
                {"Other3_12", "NetSales_12 * Other3_12 / NetSales_12"},
                {"CapexDepreciation1_12", "CapexDepreciation / 6"},
                {"AssetDisposal2_12", "Depreciation3_12 * AssetDisposal2_12 / Depreciation3_12"},
                {"CorporateOverheads_12", "Chainadministrativecosts"},
                {"Chainadministrativecosts_12", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_12", "EmployeeCosts1_12 * EmployeeCosts4_12 / EmployeeCosts1_12"},
                {"Other_12", "NetSales_12 * Other_12 / NetSales_12"},
                {"Allocations_12", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_12",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_12", "GrossProfit_12 * CorporateallocationsOther_12 / GrossProfit_12"},
                {"HeadOfficeRentalSavings_12", "0"},
                {"ITCosts_12", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_12", "GrossProfit_12 * Cellularallocations_12 / GrossProfit_12"},
                {"CreditAndFSallocations_12", "GrossProfit_12 * CreditAndFSallocations_12 / GrossProfit_12"},
                {"GrossProfit_12", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_12",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_12", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_12",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_12",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_12", "Depreciation1_12 * Corporatedepreciation_12 / Depreciation1_12"},
                {"Corporateamortisationofgoodwillandintangibleassets2_12", "0"},
                {"Interest_12", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_12", "0"},
                {"Financinginterest_12", "0"},
                {"EBIT_12", "GrossProfit + TotalExpenses"},
                {"EBITDA_12", "EBIT - DepreciationandAmmortisation"},
                {"EBT_12", "EBIT + Interest"},
                {"Tax_12", "CorporateTaxRate * EBT"},
                {"NetEarnings_12", "EBT - Tax"},
                {"PercentofRevenue_12", "GrossProfit / NetSales"},
                {"HyperionGrossSales_13", "0"},
                {
                    "NetStores_13",
                    "CurrentNetStores_13 + NumberOfOpenedStores_13 - NumberOfClosedStores_13 - NumberOfSoldStores_13 + NumberOfStoresConvertedAcrossFormats_13"
                },
                {"EmployeeWageInflation_13", "EmployeeWageInflation_12 * (1 + EmployeeWageInflationFactor_13)"},
                {"RentalInflation_13", "RentalInflation_12 * (1 + RentalInflationFactor_13)"},
                {"StoreCapexInflation_13", "StoreCapexInflation_12 * (1 + StoreCapexInflationFactor_13)"},
                {"SalesGrowth_13", "SalesGrowth_12 * (1 + SalesGrowthFactor_13)"},
                {
                    "ClosingCannibilisationEffect_13",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_13",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_13",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_13", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_13",
                    "(SalesGrowth_13) * (NetStores_13 * TradingDensity_13 * POW[1 + TradingDensityFactor_13, 1] * AverageStoreSize_13 / 1000000)"
                },
                {"Markdowns_13", "-1 * GrossSales_13 * MarkdownsPercentOfHyperionGrossSales_13 * MarkdownsFactor_13"},
                {"Promotions_13", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_13", "GrossSales_13 * LoyaltyDiscount_13 / GrossSales_13"},
                {"CreditSales_13", "NetSales_13 * CreditSales_13 / NetSales_13"},
                {"CashSales_13", "NetSales - CreditSales"},
                {"TotalCostofSales_13", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_13", "RetekGrossProfit"},
                {"RetekGrossProfit_13", "NetSales_13 * RetekGrossProfit_13 / NetSales_13"},
                {"IndirectCostofSales_13", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_13", "NetSales_13 * TCPSupplyChainCost_13 / NetSales_13"},
                {"Rebates_13", "NetSales_13 * Rebates_13 / NetSales_13"},
                {
                    "OtherAdjustments_13",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_13", "DirectCostofSales_13 * DiscountsandFGOs_13 / DirectCostofSales_13"},
                {"CentralStoreReturns_13", "DirectCostofSales_13 * CentralStoreReturns_13 / DirectCostofSales_13"},
                {"SupplierAdjustments_13", "DirectCostofSales_13 * SupplierAdjustments_13 / DirectCostofSales_13"},
                {"OtherGrossProfitAdjustments_13", "NetSales_13 * OtherGrossProfitAdjustments_13 / NetSales_13"},
                {"ConcessionFee_13", "NetSales_13 * ConcessionFee_13 / NetSales_13"},
                {"Advertising_13", "NetSales_13 * Advertising_13 / NetSales_13"},
                {
                    "TotalExpenses_13",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_13", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_13",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_13", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_13", "NetSales_13 * Stocklosses_13 / NetSales_13"},
                {"Rent_13", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_13", "CreditSales_13 * Creditcardcommisions_13 / CreditSales_13"},
                {"Security_13", "NetSales_13 * Security_13 / NetSales_13"},
                {"Cleaning_13", "NetSales_13 * Cleaning_13 / NetSales_13"},
                {"Printingandstationery_13", "NetSales_13 * Printingandstationery_13 / NetSales_13"},
                {"Communications_13", "NetSales_13 * Communications_13 / NetSales_13"},
                {"WaterAndelectricity_13", "Rent_13 * WaterAndelectricity_13 / Rent_13"},
                {"RepairsAndmaintenance_13", "Rent_13 * RepairsAndmaintenance_13 / Rent_13"},
                {"Other1_13", "NetSales_13 * Other1_13 / NetSales_13"},
                {
                    "StoreNonCashCosts_13",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_13", "Rent_13 * Operatingleaseadjustment_13 / Rent_13"},
                {"Onerousleasecredit_13", "Rent_13 * Onerousleasecredit_13 / Rent_13"},
                {"Depreciation1_13", "NetSales_13 * Depreciation1_13 / NetSales_13"},
                {"Amortisationofgoodwillandintangibleassets1_13", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_13",
                    "Depreciation1_13 * WriteoffofpropertyplantAndequipment_13 / Depreciation1_13"
                },
                {"ABSAMerchantFee_13", "CreditSales_13 * ABSAMerchantFee_13 / CreditSales_13"},
                {"ClubProfit_13", "NetSales_13 * ClubProfit_13 / NetSales_13"},
                {"SharedServices_13", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_13",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_13", "EmployeeCosts1_13 * EmployeeCosts2_13 / EmployeeCosts1_13"},
                {"Depreciation2_13", "Depreciation1_13 * Depreciation2_13 / Depreciation1_13"},
                {"Amortisationofgoodwillandintangibleassets_13", "0"},
                {"AssetDisposal1_13", "EmployeeCosts2_13 * AssetDisposal1_13 / EmployeeCosts2_13"},
                {"Travel1_13", "GrossProfit_13 * Travel1_13 / GrossProfit_13"},
                {"Other2_13", "NetSales_13 * Other2_13 / NetSales_13"},
                {
                    "Chainmanagementmerchandisse_13",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_13", "EmployeeCosts1_13 * EmployeeCosts3_13 / EmployeeCosts1_13"},
                {"Travel2_13", "GrossProfit_13 * Travel2_13 / GrossProfit_13"},
                {"Depreciation3_13", "Depreciation1_13 * Depreciation3_13 / Depreciation1_13"},
                {"Other3_13", "NetSales_13 * Other3_13 / NetSales_13"},
                {"CapexDepreciation1_13", "CapexDepreciation / 6"},
                {"AssetDisposal2_13", "Depreciation3_13 * AssetDisposal2_13 / Depreciation3_13"},
                {"CorporateOverheads_13", "Chainadministrativecosts"},
                {"Chainadministrativecosts_13", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_13", "EmployeeCosts1_13 * EmployeeCosts4_13 / EmployeeCosts1_13"},
                {"Other_13", "NetSales_13 * Other_13 / NetSales_13"},
                {"Allocations_13", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_13",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_13", "GrossProfit_13 * CorporateallocationsOther_13 / GrossProfit_13"},
                {"HeadOfficeRentalSavings_13", "0"},
                {"ITCosts_13", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_13", "GrossProfit_13 * Cellularallocations_13 / GrossProfit_13"},
                {"CreditAndFSallocations_13", "GrossProfit_13 * CreditAndFSallocations_13 / GrossProfit_13"},
                {"GrossProfit_13", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_13",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_13", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_13",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_13",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_13", "Depreciation1_13 * Corporatedepreciation_13 / Depreciation1_13"},
                {"Corporateamortisationofgoodwillandintangibleassets2_13", "0"},
                {"Interest_13", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_13", "0"},
                {"Financinginterest_13", "0"},
                {"EBIT_13", "GrossProfit + TotalExpenses"},
                {"EBITDA_13", "EBIT - DepreciationandAmmortisation"},
                {"EBT_13", "EBIT + Interest"},
                {"Tax_13", "CorporateTaxRate * EBT"},
                {"NetEarnings_13", "EBT - Tax"},
                {"PercentofRevenue_13", "GrossProfit / NetSales"},
                {"HyperionGrossSales_14", "0"},
                {
                    "NetStores_14",
                    "CurrentNetStores_14 + NumberOfOpenedStores_14 - NumberOfClosedStores_14 - NumberOfSoldStores_14 + NumberOfStoresConvertedAcrossFormats_14"
                },
                {"EmployeeWageInflation_14", "EmployeeWageInflation_13 * (1 + EmployeeWageInflationFactor_14)"},
                {"RentalInflation_14", "RentalInflation_13 * (1 + RentalInflationFactor_14)"},
                {"StoreCapexInflation_14", "StoreCapexInflation_13 * (1 + StoreCapexInflationFactor_14)"},
                {"SalesGrowth_14", "SalesGrowth_13 * (1 + SalesGrowthFactor_14)"},
                {
                    "ClosingCannibilisationEffect_14",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_14",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_14",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_14", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_14",
                    "(SalesGrowth_14) * (NetStores_14 * TradingDensity_14 * POW[1 + TradingDensityFactor_14, 1] * AverageStoreSize_14 / 1000000)"
                },
                {"Markdowns_14", "-1 * GrossSales_14 * MarkdownsPercentOfHyperionGrossSales_14 * MarkdownsFactor_14"},
                {"Promotions_14", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_14", "GrossSales_14 * LoyaltyDiscount_14 / GrossSales_14"},
                {"CreditSales_14", "NetSales_14 * CreditSales_14 / NetSales_14"},
                {"CashSales_14", "NetSales - CreditSales"},
                {"TotalCostofSales_14", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_14", "RetekGrossProfit"},
                {"RetekGrossProfit_14", "NetSales_14 * RetekGrossProfit_14 / NetSales_14"},
                {"IndirectCostofSales_14", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_14", "NetSales_14 * TCPSupplyChainCost_14 / NetSales_14"},
                {"Rebates_14", "NetSales_14 * Rebates_14 / NetSales_14"},
                {
                    "OtherAdjustments_14",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_14", "DirectCostofSales_14 * DiscountsandFGOs_14 / DirectCostofSales_14"},
                {"CentralStoreReturns_14", "DirectCostofSales_14 * CentralStoreReturns_14 / DirectCostofSales_14"},
                {"SupplierAdjustments_14", "DirectCostofSales_14 * SupplierAdjustments_14 / DirectCostofSales_14"},
                {"OtherGrossProfitAdjustments_14", "NetSales_14 * OtherGrossProfitAdjustments_14 / NetSales_14"},
                {"ConcessionFee_14", "NetSales_14 * ConcessionFee_14 / NetSales_14"},
                {"Advertising_14", "NetSales_14 * Advertising_14 / NetSales_14"},
                {
                    "TotalExpenses_14",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_14", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_14",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_14", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_14", "NetSales_14 * Stocklosses_14 / NetSales_14"},
                {"Rent_14", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_14", "CreditSales_14 * Creditcardcommisions_14 / CreditSales_14"},
                {"Security_14", "NetSales_14 * Security_14 / NetSales_14"},
                {"Cleaning_14", "NetSales_14 * Cleaning_14 / NetSales_14"},
                {"Printingandstationery_14", "NetSales_14 * Printingandstationery_14 / NetSales_14"},
                {"Communications_14", "NetSales_14 * Communications_14 / NetSales_14"},
                {"WaterAndelectricity_14", "Rent_14 * WaterAndelectricity_14 / Rent_14"},
                {"RepairsAndmaintenance_14", "Rent_14 * RepairsAndmaintenance_14 / Rent_14"},
                {"Other1_14", "NetSales_14 * Other1_14 / NetSales_14"},
                {
                    "StoreNonCashCosts_14",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_14", "Rent_14 * Operatingleaseadjustment_14 / Rent_14"},
                {"Onerousleasecredit_14", "Rent_14 * Onerousleasecredit_14 / Rent_14"},
                {"Depreciation1_14", "NetSales_14 * Depreciation1_14 / NetSales_14"},
                {"Amortisationofgoodwillandintangibleassets1_14", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_14",
                    "Depreciation1_14 * WriteoffofpropertyplantAndequipment_14 / Depreciation1_14"
                },
                {"ABSAMerchantFee_14", "CreditSales_14 * ABSAMerchantFee_14 / CreditSales_14"},
                {"ClubProfit_14", "NetSales_14 * ClubProfit_14 / NetSales_14"},
                {"SharedServices_14", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_14",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_14", "EmployeeCosts1_14 * EmployeeCosts2_14 / EmployeeCosts1_14"},
                {"Depreciation2_14", "Depreciation1_14 * Depreciation2_14 / Depreciation1_14"},
                {"Amortisationofgoodwillandintangibleassets_14", "0"},
                {"AssetDisposal1_14", "EmployeeCosts2_14 * AssetDisposal1_14 / EmployeeCosts2_14"},
                {"Travel1_14", "GrossProfit_14 * Travel1_14 / GrossProfit_14"},
                {"Other2_14", "NetSales_14 * Other2_14 / NetSales_14"},
                {
                    "Chainmanagementmerchandisse_14",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_14", "EmployeeCosts1_14 * EmployeeCosts3_14 / EmployeeCosts1_14"},
                {"Travel2_14", "GrossProfit_14 * Travel2_14 / GrossProfit_14"},
                {"Depreciation3_14", "Depreciation1_14 * Depreciation3_14 / Depreciation1_14"},
                {"Other3_14", "NetSales_14 * Other3_14 / NetSales_14"},
                {"CapexDepreciation1_14", "CapexDepreciation / 6"},
                {"AssetDisposal2_14", "Depreciation3_14 * AssetDisposal2_14 / Depreciation3_14"},
                {"CorporateOverheads_14", "Chainadministrativecosts"},
                {"Chainadministrativecosts_14", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_14", "EmployeeCosts1_14 * EmployeeCosts4_14 / EmployeeCosts1_14"},
                {"Other_14", "NetSales_14 * Other_14 / NetSales_14"},
                {"Allocations_14", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_14",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_14", "GrossProfit_14 * CorporateallocationsOther_14 / GrossProfit_14"},
                {"HeadOfficeRentalSavings_14", "0"},
                {"ITCosts_14", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_14", "GrossProfit_14 * Cellularallocations_14 / GrossProfit_14"},
                {"CreditAndFSallocations_14", "GrossProfit_14 * CreditAndFSallocations_14 / GrossProfit_14"},
                {"GrossProfit_14", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_14",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_14", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_14",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_14",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_14", "Depreciation1_14 * Corporatedepreciation_14 / Depreciation1_14"},
                {"Corporateamortisationofgoodwillandintangibleassets2_14", "0"},
                {"Interest_14", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_14", "0"},
                {"Financinginterest_14", "0"},
                {"EBIT_14", "GrossProfit + TotalExpenses"},
                {"EBITDA_14", "EBIT - DepreciationandAmmortisation"},
                {"EBT_14", "EBIT + Interest"},
                {"Tax_14", "CorporateTaxRate * EBT"},
                {"NetEarnings_14", "EBT - Tax"},
                {"PercentofRevenue_14", "GrossProfit / NetSales"},
                {"HyperionGrossSales_15", "0"},
                {
                    "NetStores_15",
                    "CurrentNetStores_15 + NumberOfOpenedStores_15 - NumberOfClosedStores_15 - NumberOfSoldStores_15 + NumberOfStoresConvertedAcrossFormats_15"
                },
                {"EmployeeWageInflation_15", "EmployeeWageInflation_14 * (1 + EmployeeWageInflationFactor_15)"},
                {"RentalInflation_15", "RentalInflation_14 * (1 + RentalInflationFactor_15)"},
                {"StoreCapexInflation_15", "StoreCapexInflation_14 * (1 + StoreCapexInflationFactor_15)"},
                {"SalesGrowth_15", "SalesGrowth_14 * (1 + SalesGrowthFactor_15)"},
                {
                    "ClosingCannibilisationEffect_15",
                    "ClosedStoreCannibilisationFactor * NumberOfClosedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "OpeningCannibilisationEffect_15",
                    "OpenedStoreCannibilisationFactor * NumberOfOpenedStores * TradingDensity * (1 + TradingDensityFactor) * AverageStoreSize"
                },
                {
                    "CannibilisationEffect_15",
                    "(SalesGrowth) * (ClosingCannibilisationEffect - OpeningCannibilisationEffect)"
                },
                {"NetSales_15", "GrossSales+Markdowns+Promotions+LoyaltyDiscount"},
                {
                    "GrossSales_15",
                    "(SalesGrowth_15) * (NetStores_15 * TradingDensity_15 * POW[1 + TradingDensityFactor_15, 1] * AverageStoreSize_15 / 1000000)"
                },
                {"Markdowns_15", "-1 * GrossSales_15 * MarkdownsPercentOfHyperionGrossSales_15 * MarkdownsFactor_15"},
                {"Promotions_15", "-1 * GrossSales * MarkdownsPercentOfHyperionGrossSales * PromotionsFactor"},
                {"LoyaltyDiscount_15", "GrossSales_15 * LoyaltyDiscount_15 / GrossSales_15"},
                {"CreditSales_15", "NetSales_15 * CreditSales_15 / NetSales_15"},
                {"CashSales_15", "NetSales - CreditSales"},
                {"TotalCostofSales_15", "DirectCostofSales+IndirectCostofSales"},
                {"DirectCostofSales_15", "RetekGrossProfit"},
                {"RetekGrossProfit_15", "NetSales_15 * RetekGrossProfit_15 / NetSales_15"},
                {"IndirectCostofSales_15", "TCPSupplyChainCost+Rebates+OtherAdjustments+ConcessionFee+Advertising"},
                {"TCPSupplyChainCost_15", "NetSales_15 * TCPSupplyChainCost_15 / NetSales_15"},
                {"Rebates_15", "NetSales_15 * Rebates_15 / NetSales_15"},
                {
                    "OtherAdjustments_15",
                    "DiscountsandFGOs+CentralStoreReturns+SupplierAdjustments+OtherGrossProfitAdjustments"
                },
                {"DiscountsandFGOs_15", "DirectCostofSales_15 * DiscountsandFGOs_15 / DirectCostofSales_15"},
                {"CentralStoreReturns_15", "DirectCostofSales_15 * CentralStoreReturns_15 / DirectCostofSales_15"},
                {"SupplierAdjustments_15", "DirectCostofSales_15 * SupplierAdjustments_15 / DirectCostofSales_15"},
                {"OtherGrossProfitAdjustments_15", "NetSales_15 * OtherGrossProfitAdjustments_15 / NetSales_15"},
                {"ConcessionFee_15", "NetSales_15 * ConcessionFee_15 / NetSales_15"},
                {"Advertising_15", "NetSales_15 * Advertising_15 / NetSales_15"},
                {
                    "TotalExpenses_15",
                    "StoreCosts+ABSAMerchantFee+ClubProfit+SharedServices+CorporateOverheads+Allocations"
                },
                {"StoreCosts_15", "StoreCashCosts+StoreNonCashCosts"},
                {
                    "StoreCashCosts_15",
                    "EmployeeCosts1+Stocklosses+Rent+Creditcardcommisions+Security+Cleaning+Printingandstationery+Communications+WaterAndelectricity+RepairsAndmaintenance+Other1"
                },
                {"EmployeeCosts1_15", "NetStores * AverageStoreEmployeeCosts * EmployeeWageInflation"},
                {"Stocklosses_15", "NetSales_15 * Stocklosses_15 / NetSales_15"},
                {"Rent_15", "NetStores * AverageStoreRent * RentalInflation"},
                {"Creditcardcommisions_15", "CreditSales_15 * Creditcardcommisions_15 / CreditSales_15"},
                {"Security_15", "NetSales_15 * Security_15 / NetSales_15"},
                {"Cleaning_15", "NetSales_15 * Cleaning_15 / NetSales_15"},
                {"Printingandstationery_15", "NetSales_15 * Printingandstationery_15 / NetSales_15"},
                {"Communications_15", "NetSales_15 * Communications_15 / NetSales_15"},
                {"WaterAndelectricity_15", "Rent_15 * WaterAndelectricity_15 / Rent_15"},
                {"RepairsAndmaintenance_15", "Rent_15 * RepairsAndmaintenance_15 / Rent_15"},
                {"Other1_15", "NetSales_15 * Other1_15 / NetSales_15"},
                {
                    "StoreNonCashCosts_15",
                    "Operatingleaseadjustment+Onerousleasecredit+Depreciation1+Amortisationofgoodwillandintangibleassets1+WriteoffofpropertyplantAndequipment"
                },
                {"Operatingleaseadjustment_15", "Rent_15 * Operatingleaseadjustment_15 / Rent_15"},
                {"Onerousleasecredit_15", "Rent_15 * Onerousleasecredit_15 / Rent_15"},
                {"Depreciation1_15", "NetSales_15 * Depreciation1_15 / NetSales_15"},
                {"Amortisationofgoodwillandintangibleassets1_15", "0"},
                {
                    "WriteoffofpropertyplantAndequipment_15",
                    "Depreciation1_15 * WriteoffofpropertyplantAndequipment_15 / Depreciation1_15"
                },
                {"ABSAMerchantFee_15", "CreditSales_15 * ABSAMerchantFee_15 / CreditSales_15"},
                {"ClubProfit_15", "NetSales_15 * ClubProfit_15 / NetSales_15"},
                {"SharedServices_15", "Chainmanagementoperations+Chainmanagementmerchandisse"},
                {
                    "Chainmanagementoperations_15",
                    "EmployeeCosts2+Depreciation2+Amortisationofgoodwillandintangibleassets+AssetDisposal1+Travel1+Other2"
                },
                {"EmployeeCosts2_15", "EmployeeCosts1_15 * EmployeeCosts2_15 / EmployeeCosts1_15"},
                {"Depreciation2_15", "Depreciation1_15 * Depreciation2_15 / Depreciation1_15"},
                {"Amortisationofgoodwillandintangibleassets_15", "0"},
                {"AssetDisposal1_15", "EmployeeCosts2_15 * AssetDisposal1_15 / EmployeeCosts2_15"},
                {"Travel1_15", "GrossProfit_15 * Travel1_15 / GrossProfit_15"},
                {"Other2_15", "NetSales_15 * Other2_15 / NetSales_15"},
                {
                    "Chainmanagementmerchandisse_15",
                    "EmployeeCosts3+Travel2+Depreciation3+Other3+CapexDepreciation1+AssetDisposal2"
                },
                {"EmployeeCosts3_15", "EmployeeCosts1_15 * EmployeeCosts3_15 / EmployeeCosts1_15"},
                {"Travel2_15", "GrossProfit_15 * Travel2_15 / GrossProfit_15"},
                {"Depreciation3_15", "Depreciation1_15 * Depreciation3_15 / Depreciation1_15"},
                {"Other3_15", "NetSales_15 * Other3_15 / NetSales_15"},
                {"CapexDepreciation1_15", "CapexDepreciation / 6"},
                {"AssetDisposal2_15", "Depreciation3_15 * AssetDisposal2_15 / Depreciation3_15"},
                {"CorporateOverheads_15", "Chainadministrativecosts"},
                {"Chainadministrativecosts_15", "EmployeeCosts4+Other"},
                {"EmployeeCosts4_15", "EmployeeCosts1_15 * EmployeeCosts4_15 / EmployeeCosts1_15"},
                {"Other_15", "NetSales_15 * Other_15 / NetSales_15"},
                {"Allocations_15", "CorporatecreditAndfinancialservicesallocations"},
                {
                    "CorporatecreditAndfinancialservicesallocations_15",
                    "CorporateallocationsOther+HeadOfficeRentalSavings+ITCosts+Cellularallocations+CreditAndFSallocations"
                },
                {"CorporateallocationsOther_15", "GrossProfit_15 * CorporateallocationsOther_15 / GrossProfit_15"},
                {"HeadOfficeRentalSavings_15", "0"},
                {"ITCosts_15", "ITCostsPercentage * NetSales"},
                {"Cellularallocations_15", "GrossProfit_15 * Cellularallocations_15 / GrossProfit_15"},
                {"CreditAndFSallocations_15", "GrossProfit_15 * CreditAndFSallocations_15 / GrossProfit_15"},
                {"GrossProfit_15", "NetSales + TotalCostofSales"},
                {
                    "DepreciationandAmmortisation_15",
                    "ChainDepreciation+ChainwriteoffofpropertyplantAndequipment+CapexDepreciation2+Corporatedepreciation+Corporateamortisationofgoodwillandintangibleassets2"
                },
                {"ChainDepreciation_15", "Depreciation1 + Depreciation2 + Depreciation3"},
                {
                    "ChainwriteoffofpropertyplantAndequipment_15",
                    "WriteoffofpropertyplantAndequipment + AssetDisposal1 + AssetDisposal2"
                },
                {
                    "CapexDepreciation2_15",
                    "Amortisationofgoodwillandintangibleassets1 + Amortisationofgoodwillandintangibleassets + CapexDepreciation1"
                },
                {"Corporatedepreciation_15", "Depreciation1_15 * Corporatedepreciation_15 / Depreciation1_15"},
                {"Corporateamortisationofgoodwillandintangibleassets2_15", "0"},
                {"Interest_15", "Workingcapitalinterest+Financinginterest"},
                {"Workingcapitalinterest_15", "0"},
                {"Financinginterest_15", "0"},
                {"EBIT_15", "GrossProfit + TotalExpenses"},
                {"EBITDA_15", "EBIT - DepreciationandAmmortisation"},
                {"EBT_15", "EBIT + Interest"},
                {"Tax_15", "CorporateTaxRate * EBT"},
                {"NetEarnings_15", "EBT - Tax"},
                {"PercentofRevenue_15", "GrossProfit / NetSales"},
            };

            var variableProvider = new ExpressionVariableProvider(input, DefaultFunctionProvider.Instance);

            var sw = new Stopwatch();
            sw.Start();
            var result = variableProvider.Lookup("Markdowns_15");
            sw.Stop();
            Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");
        }
    }
}
