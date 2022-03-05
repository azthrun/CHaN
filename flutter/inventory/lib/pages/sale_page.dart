import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
import 'package:inventory/models/sale.dart';
import 'package:inventory/provider/online_data_provider.dart';

final saleStreamProvider = StreamProvider<List<Sale>>((ref) {
  final saleData = ref.watch(saleProvider);
  return saleData.allItems;
});

class SalePage extends ConsumerWidget {
  const SalePage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final stream = ref.watch(saleStreamProvider);
    return Scaffold(
      body: stream.when(
        data: _buildSalesList,
        error: (e, stack) => Text('Error: $e'),
        loading: () => const CircularProgressIndicator(),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {},
        child: const FaIcon(FontAwesomeIcons.plus),
      ),
    );
  }

  Widget _buildSalesList(List<Sale> sales) {
    return ListView(
      children: sales
          .map(
            (e) => ListTile(
              title: Text(e.inventory!.name!),
              subtitle: Text((e.price == null) ? '' : e.price!.toString()),
            ),
          )
          .toList(),
    );
  }
}
